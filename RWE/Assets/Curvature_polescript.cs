using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point_generator : MonoBehaviour
{
    public GameObject polePrefab;
    public GameObject player;
    // public GameObject plane; // the path 

    public GameObject arrow;

    public float detectionRadius = 2f;

    private GameObject currentPole;
    private Vector3[] polePoints;
    private int currentPoleIndex = 0;

    public float Random_distance_minX = -15f;
    public float Random_distance_maxX = 15f;
    public float Random_distance_minZ = -15f;
    public float Random_distance_maxZ = 15f;

    public bool Curvature_test = false;
    public bool Rotation_test = false;
    public bool Bending_test = false;
    public bool Random_test = false;


    void Start()
    {
        if (Curvature_test == true) { polePoints = Vector_manager.Curvature_points; }
        else if (Rotation_test == true) { polePoints = Vector_manager.Rotation_points; }
        else if (Bending_test == true) { polePoints = Vector_manager.Bending_points; }
        
        else if (Random_test == true) 
        { 
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

        GeneratePoleSequence();
    }

    void Update()
    {

        if (Vector3.Distance(player.transform.position, currentPole.transform.position) < detectionRadius)
        {
            if (currentPoleIndex > polePoints.Length - 1)
            {
                currentPole.SetActive(false);
                return;
            }

            GeneratePoleSequence();
        }

        if (currentPole != null && arrow != null)
        {
            // Calculate the position of the arrow just in front of the player
            Vector3 arrowPosition = player.transform.position + player.transform.forward * 2f; // Adjust 2.0f as needed
            arrowPosition.y = player.transform.position.y*1.2f; // Keep the  height doubke as the player
            arrow.transform.position = arrowPosition;

            // Set arrow rotation to point towards the pole
            Vector3 direction = new Vector3(currentPole.transform.position.x - arrow.transform.position.x, 0f, currentPole.transform.position.z - arrow.transform.position.z);
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0); 
                arrow.transform.rotation = rotation;
            }
        }
    }

    void GeneratePoleSequence()
    {
        if (currentPole != null)
        {
            currentPole.transform.position = polePoints[currentPoleIndex];
        }
        else
        {
            currentPole = Instantiate(polePrefab, polePoints[currentPoleIndex], Quaternion.identity);
            polePrefab.SetActive(false);
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
