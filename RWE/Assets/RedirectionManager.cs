
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;


public class RedirectionManager : MonoBehaviour
{
    [Tooltip("The gameobject the redirection should be applied to (Usually the object containing the main camera)")]
    public GameObject cameraOffset;
    [Tooltip("The object used for reading the player position and movements")]
    public GameObject mainCamera;

    [Tooltip("Default multiplier value for rotation gain")]
    [Range(-5f, 5f)]
    public float rotationGain;
    [Tooltip("Default multiplier value for translation gain")]
    [Range(-5f, 5f)]
    public float translationGain;
    [Tooltip("Default multiplier value for Bending gain")]
    [Range(-5f, 5f)]
    public float bendingGain;
    [Tooltip("Default multiplier value for curvature gain")]
    [Range(-5f, 5f)]
    public float curvatureGain;

    private float previousXRotation;
    private float previousRealRotation;
    private Vector3 previousPosition;
    private Vector3 previousRealPosition;

    private bool rotationActive;
    private bool translationActive;
    private bool bendingActive;
    private bool curvingActive;

    string filePath = "positions.txt";
    string usedFilePath;
    // Start is called before the first frame update
    void Start()
    {
        rotationActive = false;
        translationActive = false;
        bendingActive = false;
        curvingActive = false;

        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
        previousRealRotation = previousXRotation;
        previousRealPosition = previousPosition;

        setUpSaveToFile();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Real rotation and position
        previousRealRotation += (mainCamera.transform.rotation.eulerAngles.y - previousXRotation);
        previousRealRotation = previousRealRotation % 360;
        previousRealPosition += (mainCamera.transform.position - previousPosition);

        //Rotation
        if (rotationActive) {
            injectRotation();
        }

        //Translation
        if (translationActive) {
            injectTranslation();
        }

        //Bending
        if (bendingActive) {
            injectBending();
        }

        //Curvature  
        if (curvingActive) {
            injectCurvature();
        }

        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;

        // Save the position of cameraOffset to a file
        SavePositionToFile();
    }

    private void setUpSaveToFile(){
        int count = 1;

        usedFilePath = filePath;

        // Check if the file exists, if yes, append a number until a unique filename is found
        while (File.Exists(usedFilePath))
        {
            usedFilePath = $"{Path.GetFileNameWithoutExtension(filePath)}_{count}{Path.GetExtension(filePath)}";
            count++;
        }

        try
        {
            // Create the file
            using (FileStream fs = File.Create(usedFilePath))
            {
                // Write header to the file
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("x-coordinates;real_x-coordinates;y-coordinates;real_y-coordinates;z-coordinates;real_z-koordinates;rotation;real_rotation");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error setting up save to file: " + e.Message);
        }
    }
    private void SavePositionToFile()
    {
        try
        {
            // Open a file stream to write the positions
            using (StreamWriter writer = new StreamWriter(usedFilePath, append: true))
            {
                // Write the position to the file
                writer.WriteLine(previousPosition.x + ";" + previousRealPosition.x + ";" + previousPosition.y + ";" + previousRealPosition.y + ";" + previousPosition.z + ";" + previousRealPosition.z + ";" + previousXRotation + ";" + previousRealRotation);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving position to file: " + e.Message);
        }
    }

    private void injectRotation() 
    {
        float currentXRotation = mainCamera.transform.rotation.eulerAngles.y;

        // Calculate the change in rotation
        float change = currentXRotation - previousXRotation;

        // Adjust for full rotations
        if (Mathf.Abs(change) > 180f)
        {
            change -= Mathf.Sign(change) * 360f;
        }

        // Apply the rotation to the gameobject
        cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, rotationGain * change);
    }

    private void injectTranslation()
    {
        
        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 positionChange = translationGain*(currentPosition - previousPosition);
        cameraOffset.transform.Translate(new Vector3(positionChange.x,0,positionChange.z),Space.World);
    }

    private void injectCurvature()
    {
        float currentXRotation = mainCamera.transform.rotation.eulerAngles.y;

        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 positionChange = currentPosition - previousPosition;

        cameraOffset.transform.RotateAround(mainCamera.transform.position,Vector3.up,curvatureGain*positionChange.magnitude);
    }

    private void injectBending()
    {
        float currentXRotation = mainCamera.transform.rotation.eulerAngles.y;
        float change = currentXRotation - previousXRotation;
        if (Mathf.Abs(change) > 180f)
        {
            change -= Mathf.Sign(change) * 360f;
        }

        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 positionChange = currentPosition - previousPosition;

        cameraOffset.transform.RotateAround(mainCamera.transform.position,Vector3.up,bendingGain*positionChange.magnitude*change);
    }


    public void rotationToggled()
    {
        if(rotationActive)
        {
            rotationActive = false;
        } else {
            rotationActive = true;
        }
    }

    public void translationToggled()
    {
        if(translationActive)
        {
            translationActive = false;
        } else {
            translationActive = true;
        }
    }

    public void bendingToggled()
    {
        if(bendingActive)
        { 
            bendingActive = false; 
        } else { 
            bendingActive = true;
        } 
    }

    public void curvatureToggled()
    {
        if(curvingActive)
        {
            curvingActive = false;
        } else {
            curvingActive = true;
        }
    }
    
    public void setRotationMultiplier(float multiplier)
    {
        rotationGain = multiplier;
    }

    public void setTranslationMultiplier(float multiplier)
    {
        translationGain = multiplier;
    }

    public void setBendingMultiplier(float multiplier)
    {
        bendingGain = multiplier;
    }

    public void setCurvatureMultiplier(float multiplier)
    {
        curvatureGain = multiplier;
    }
}
