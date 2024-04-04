using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point_generator : MonoBehaviour
{
    public GameObject polePrefab;
    public GameObject player;
    // public GameObject teleportPlayer;
    // public GameObject plane; // the path 
    public GameObject arrow;

    private GameObject currentPole;
    private Vector3[] polePoints;
    private int currentPoleIndex = 0;
    public float detectionRadius = 2.6f;

    // Maximun and minimum distances for random pole positions
    public float Random_distance_minX = -10f;
    public float Random_distance_maxX = 10f;
    public float Random_distance_minZ = -10f;
    public float Random_distance_maxZ = 10f;

    public bool curvatureTest = false;
    public bool rotationTest = false;
    public bool bendingTest = false;
    public bool randomTest = false;

    private bool initialized = false;

    public void InitializeTest(bool curvatureTest, bool rotationTest, bool bendingTest, bool randomTest)
    {
        // Check which testin sequence is activated
        // Update poles into polePoints form script Vector_manager
        if (curvatureTest) { polePoints = Vector_manager.Curvature_points; }
        else if (rotationTest) { polePoints = Vector_manager.Rotation_points; }
        else if (bendingTest) { polePoints = Vector_manager.Bending_points; }

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
        }

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

        if (currentPole != null && arrow != null)
        {
            // Calculate the position of the arrow just in front of the player
            Vector3 arrowPosition = player.transform.position + player.transform.forward * 2f; // Adjust 2.0f as needed
            arrowPosition.y = player.transform.position.y * 1.2f; // Keep the  height doubke as the player
            arrow.transform.position = arrowPosition;

            // Set arrow rotation to point towards the pole
            Vector3 direction = new Vector3(currentPole.transform.position.x - arrow.transform.position.x, 0f, currentPole.transform.position.z - arrow.transform.position.z);
            if (direction != Vector3.zero)
            {
                // rotate arrow to point form the tip and not from midlle
                Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                arrow.transform.rotation = rotation;
            }
        }
    }

    public void ResetSequence()
    {
        // Deactivate all toggles
        curvatureTest = false;
        rotationTest = false;
        bendingTest = false;
        randomTest = false;

        currentPoleIndex = 0;
        
        if (currentPole != null){
            Destroy(currentPole);
            currentPole = null; 
        }
       
        initialized = false;

        // Teleport the player to the starting point
        /*
        Vector3 startingPosition = new Vector3(0f, 0f, 0f);
        player.transform.position = startingPosition;
        */
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
            polePrefab.transform.position = new Vector3(0, -10, 0);
        }

        currentPoleIndex++;
    }



    /*
    void CreatePath(Vector3 startPoint, Vector3 endPoint)
    {
        // Calculate center position of the path
        Vector3 center = (startPoint + endPoint) * 0.5f;
        center.y = 5.1f; // Set the desired height above the ground

        // Calculate direction from start to end
        Vector3 direction = (endPoint - startPoint).normalized;

        // Calculate distance between start and end
        float distance = Vector3.Distance(startPoint, endPoint);

        // Instantiate plane GameObject for the path
        GameObject pathObject = Instantiate(plane, center, Quaternion.identity);

        // Rotate plane by 90 degrees around x-axis to lay it flat on the floor
        pathObject.transform.rotation = Quaternion.Euler(180f, 0f, 0f);

        // Scale the plane to represent the path
        pathObject.transform.localScale = new Vector3(distance, 0.01f, 0.8f); // Adjust width to 0.8 meters and thickness as needed
    }
    */

}


