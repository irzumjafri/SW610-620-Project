using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point_generator : MonoBehaviour
{
    public GameObject polePrefab;
    public GameObject player;
    // public GameObject plane; // the path 

    public float detectionRadius = 2f;

    private GameObject currentPole;
    private Vector3[] polePoints;
    private int currentPoleIndex = 0;

    public bool Curvature_test = false;
    public bool Rotation_test = false;
    public bool Bending_test = false;


    void Start()
    {
        if (Curvature_test == true) { polePoints = Vector_manager.Curvature_points; }
        else if (Rotation_test == true) { polePoints = Vector_manager.Rotation_points; }
        else if (Bending_test == true) { polePoints = Vector_manager.Bending_points; }

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
