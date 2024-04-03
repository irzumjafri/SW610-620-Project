using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPole_generator : MonoBehaviour

{
    public GameObject polePrefab;
    public GameObject player;
    public GameObject arrow;

    public float detectionRadius = 2f;

    public float minX = -15f;
    public float maxX = 15f;
    public float minZ = -15f;
    public float maxZ = 15f;

    private GameObject currentPole;
    private Vector3 currentPoint;

    void Start()
    {
        GenerateRandomPoint();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, currentPoint) < detectionRadius)
        {
            GenerateRandomPoint();
        }

        if (currentPole != null && arrow != null)
        {
            // Calculate the position of the arrow just in front of the player
            Vector3 arrowPosition = player.transform.position + player.transform.forward * 2.0f; // Adjust 2.0f as needed
            arrowPosition.y = player.transform.position.y*2; // Keep the  height doubke as the player
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

    void GenerateRandomPoint()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        currentPoint = new Vector3(randomX, 0f, randomZ);

        if (currentPole != null)
        {
            currentPole.transform.position = currentPoint;
        }
        else
        {
            currentPole = Instantiate(polePrefab, currentPoint, Quaternion.identity);
            polePrefab.SetActive(false);
        }
    }
}