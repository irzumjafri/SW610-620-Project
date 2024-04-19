using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point_generator : MonoBehaviour
{
    [Tooltip("Pole to incdicate current goal")]
    public GameObject polePrefab;
    [Tooltip("Player object to check direction and rotation")]
    public GameObject circlePrefab;
    public GameObject player;
    public GameObject teleportPlayer; // to teleport the player to the starting position after sewuence is complete
    [Tooltip("Path to show current path the player walks")]
    public GameObject path;
    [Tooltip("Points to the curren goal pole")]
    public GameObject arrow;
    public GameObject circlePathoverlay;

    public float SizeOfMap = 50;
    private GameObject currentPath;
    private GameObject circlePath;
    private GameObject currentArrow;
    private GameObject currentPole;
    private GameObject pathOverlay;
    private Vector3[] polePoints;
    private int currentPoleIndex = 0;

    [Tooltip("How close the player needs to be to the pole")]
    public float detectionRadius = 2.6f;

    // Maximun and minimum distances for random pole positions
    [Tooltip("Minimum distance to generate random goalpole from center")]
    public float Random_distance_minX = -10f;
    [Tooltip("Maximum distance to generate random goalpole from center")]
    public float Random_distance_maxX = 10f;
    [Tooltip("Minimum distance to generate random goalpole from center")]
    public float Random_distance_minZ = -10f;
    [Tooltip("Maximum distance to generate random goalpole from center")]
    public float Random_distance_maxZ = 10f;

    [Tooltip("Boolean to indicate is curvature testsequence active")]
    public bool curTest;
    [Tooltip("Boolean to indicate is rotation testsequence active")]
    public bool rotTest;
    [Tooltip("Boolean to indicate is bending testsequence active")]
    public bool bendTest;
    [Tooltip("Boolean to indicate is random testsequence active")]
    public bool randTest;

    private bool initialized = false;

    public void InitializeTest(bool curvatureTest, bool rotationTest, bool bendingTest, bool randomTest)
    {
        // Check which testin sequence is activated
        // Update poles into polePoints form script Vector_manager
        if (curvatureTest) 
        { 
            polePoints = Vector_manager.Curvature_points;
            curTest = curvatureTest;
        }
        else if (rotationTest) 
        { 
            polePoints = Vector_manager.Rotation_points;
            rotTest = rotationTest;
        }
        else if (bendingTest)
        { 
            polePoints = Vector_manager.Bending_points; 
            bendTest = bendingTest; 
        }

        else if (randomTest)
        {
            // generate random points in the min-max area
            List<Vector3> Random_points = new List<Vector3>();
            for (int i = 0; i < 10; i++)
            {
                float randomZ = Random.Range(Random_distance_minX, Random_distance_maxX);
                float randomX = Random.Range(Random_distance_minZ, Random_distance_maxZ);
                Vector3 point = new Vector3(randomX, 0f, randomZ);

                Random_points.Add(point);
            }
            polePoints = Random_points.ToArray(); // Convert list to array
            randTest = randomTest;    
        }
        //updatePlayArea();
        // start putting poles into the playarea
        polePrefab.SetActive(true);
        initialized = true;
        GeneratePoleSequence();
        
    }

    void Update()
    {
        if (!initialized) // Check if initialized is false
            return;

        // Check if player is colse to the pole
        if (Vector3.Distance(player.transform.position, currentPole.transform.position) < detectionRadius)
        {
            if (currentPoleIndex > polePoints.Length - 1)
            {
                // if whole sequence is done remove poles from playarea
                ResetSequence();
                return;
            }

            // generate a new pole
            GeneratePoleSequence();
        }

        if (currentPole != null && currentArrow != null)
        {
            UpdateArrow();
        }
    }

    public void ResetSequence()
    {
        // Deactivate all toggles
        curTest = false;
        rotTest = false;
        bendTest = false;
        randTest = false;

        currentPoleIndex = 0;
        
        if (currentPole != null){
            Destroy(currentPole);
            currentPole = null; 
        }

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }

        if (currentPath != null)
        {
            Destroy(currentPath);
            currentPath= null;
        }

        if (circlePath != null)
        {
            Destroy(circlePath);
            circlePath = null;
        }

        if (pathOverlay != null)
        {
            Destroy(pathOverlay);
            pathOverlay = null;
        }

        initialized = false;

        Destroy(currentPath);

    }

    void GeneratePoleSequence()
    {
        if (currentPole != null)
        {
            // put the pole into the next place
            currentPole.transform.position = polePoints[currentPoleIndex];
        }
        // if the pole is first one set the example pole not active
        else
        {
            currentPole = Instantiate(polePrefab, polePoints[currentPoleIndex], Quaternion.identity);
            currentArrow = Instantiate(arrow, polePoints[currentPoleIndex], Quaternion.identity);
        }

        if (bendTest) { CreateCirclePath(); }
        else { CreatePath(); }
        

        currentPoleIndex++;

        if (currentPoleIndex == polePoints.Length+1)
        {
            // Destroy the circle path and its overlay
            Destroy(pathOverlay);
            Destroy(circlePath);
        }


    }

    void UpdateArrow() {
        // Calculate the position of the arrow just in front of the player
        Vector3 arrowPosition = player.transform.position + player.transform.forward * 2f; // Adjust 2.0f as needed
        arrowPosition.y = player.transform.position.y * 1.2f; // Keep the height double as the player
        currentArrow.transform.position = arrowPosition;

        // Set arrow rotation to point towards the pole
        Vector3 direction = new Vector3(currentPole.transform.position.x - currentArrow.transform.position.x, 0f, currentPole.transform.position.z - currentArrow.transform.position.z);
        if (direction != Vector3.zero)
        {
            // Rotate arrow to point from the tip and not from the middle
            Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
            currentArrow.transform.rotation = rotation;
        }
    }

    void CreatePath()
    {
        Vector3 startPoint;
        if (currentPath == null)
        {
            startPoint = new Vector3(0, 0, 0);
        }
        else
        {
            Destroy(currentPath);
            startPoint = polePoints[currentPoleIndex - 1];
        }
        Vector3 endPoint = polePoints[currentPoleIndex];

        // Calculate center position of the path
        Vector3 center = (startPoint + endPoint) * 0.5f;
        center.y += 0.13f; // lift the path on top of the ground 

        // Calculate direction and distance from start to end
        Vector3 direction = (endPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, endPoint);

        // Instantiate plane GameObject for the path
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
        currentPath = Instantiate(path, center, rotation);

        // Scale the plane to represent the path
        currentPath.transform.localScale = new Vector3(distance*0.11f, 1f, 0.08f); // values can be changed depending the scale of path wanted
    }

    void CreateCirclePath()
    {
        if (currentPoleIndex == 0)
        {

            CreatePath();
        }
        else if (currentPoleIndex == 1)
        {
            Destroy(currentPath);
            Vector3 startPoint = new Vector3(0, 0.13f, 0);
            Vector3 endPoint = polePoints[currentPoleIndex];

            float radius = Vector3.Distance(startPoint, endPoint);

            // Instantiate the circle path and its overlay
            circlePath = Instantiate(circlePrefab, startPoint, Quaternion.identity);
            pathOverlay = Instantiate(circlePathoverlay, startPoint, Quaternion.identity);

            // Set the scale of the circle path and its overlay
            circlePath.transform.localScale = new Vector3(radius * 2.1f, circlePath.transform.localScale.y, radius * 2.1f);
            pathOverlay.transform.localScale = new Vector3(radius * 1.9f, circlePathoverlay.transform.localScale.y, radius * 1.9f);
        }

        else
        {
            return; // No action needed for other conditions
        }
    }

    private void updatePlayArea() {
        
        double originalMapSize = 100.0;
        // Calculate the scaling factor based on the size of the map
        double scaleFactor = SizeOfMap / originalMapSize;

        // Iterate through each point in the Rotation_points array and scale its coordinates
        for (int i = 0; i < polePoints.Length; i++)
        {
            Vector3 point = polePoints[i];
            point.x *= (float)scaleFactor;
            point.z *= (float)scaleFactor;
            polePoints[i] = point;
        }
    }

    public void TeleportToStart() {
        Vector3 currentPosition = player.transform.position;
        Vector3 positionChange = -currentPosition;
        teleportPlayer.transform.Translate(new Vector3(positionChange.x, 0, positionChange.z), Space.World);
    }

}



