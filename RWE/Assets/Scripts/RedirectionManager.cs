
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using Firebase;
using Firebase.Database;


public class RedirectionManager : MonoBehaviour
{

    [Tooltip("The gameobject the redirection should be applied to (Usually the object containing the main camera)")]
    public GameObject cameraOffset;
    [Tooltip("The object used for reading the player position and movements")]
    public GameObject mainCamera;
    [SerializeField]
    [Tooltip("Default multiplier value for rotation gain")]
    [Range(-5f, 5f)]
    public float rotationGain;
    [SerializeField]
    [Tooltip("Default multiplier value for translation gain")]
    [Range(-5f, 5f)]
    private float translationGain;
    [SerializeField]
    [Tooltip("Default multiplier value for curvature gain")]
    [Range(-5f, 15f)]
    private float curvatureGain;
    [SerializeField]
    [Tooltip("Default multiplier value for Bending gain")]
    [Range(-5f, 20f)]
    private float bendingGain;

    private float previousXRotation;
    private float previousRealRotation;
    private Vector3 previousPosition;
    private Vector3 previous2Position;
    private Vector3 previousRealPosition;

    private bool rotationActive;
    private bool translationActive;
    private bool curvingActive;
    private bool bendingActive;

    //For firebase
    private FirebaseFirestore db;


    string usedFilePath;
    // Start is called before the first frame update
    void Start()
    {
        rotationActive = false;
        translationActive = false;
        bendingActive = false;
        curvingActive = false;

        //Initializing Database for Firebase
        db = FirebaseFirestore.GetInstance();

        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
        previousRealRotation = previousXRotation;
        previousRealPosition = previousPosition;

        setUpSaveToFile();
        InvokeRepeating("SavePositionToFile", 0.5F, 0.5F)
    }

    // Update is called once per frame
    void Update()
    {
        // Real rotation and position
        updateRealPosition();

        //Rotation
        if (rotationActive)
        {
            injectRotation();
        }

        //Translation
        if (translationActive)
        {
            injectTranslation();
        }

        //Bending
        if (bendingActive)
        {
            injectBending();
        }

        //Curvature  
        if (curvingActive)
        {
            injectCurvature();
        }

        UpdatePreviousPosition();

    }

    public void updateRealPosition()
    {
        previousRealRotation += (mainCamera.transform.rotation.eulerAngles.y - previousXRotation);
        previousRealRotation = previousRealRotation % 360;
        previousRealPosition += (mainCamera.transform.position - previousPosition);
    }

    public void UpdatePreviousPosition()
    {
        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
    }

    private void setUpSaveToFile()
    {
        usedFilePath = Path.Join(Application.persistentDataPath, string.Format("positions-{0}.txt", DateTime.Now.ToString()));
        Debug.Log(usedFilePath);

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
        SendFirebaseData(previousPosition.x, previousRealPosition.x, previousPosition.z, previousRealPosition.z, previousXRotation, previousRealRotation);
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
        Vector3 positionChange = translationGain * (currentPosition - previousPosition);
        cameraOffset.transform.Translate(new Vector3(positionChange.x, 0, positionChange.z), Space.World);
    }

    private void injectCurvature()
    {
        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 positionChange = currentPosition - previousPosition;

        cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, curvatureGain * positionChange.magnitude);
    }

    private void injectBending()
    {
        float currentXRotation = mainCamera.transform.rotation.eulerAngles.y;
        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 positionChange = currentPosition - previousPosition;

        // Calculate the bending gain effect based on the user's head rotation
        float bend = Mathf.DeltaAngle(currentXRotation, previousXRotation);



        Debug.Log("angle: " + bend.ToString("F10"));

        if (bend < 0)
        {
            cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, -bendingGain * positionChange.magnitude * Math.Abs(bend));
        }
        else if (bend > 0)
        {
            cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, bendingGain * positionChange.magnitude * Math.Abs(bend));
        }
    }

    private Vector3 RotateVectorAroundAxis(Vector3 vector, Vector3 axis, float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        float cosAngle = (float)Math.Cos(angleRad);
        float sinAngle = (float)Math.Sin(angleRad);

        return cosAngle * vector +
            (1 - cosAngle) * Vector3.Dot(axis, vector) * axis +
            sinAngle * Vector3.Cross(axis, vector);

    }

    private Vector3 RotatePointAroundOrigin(Vector3 point, float angle)
    {
        // Create a quaternion representing the rotation
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Rotate the point using the quaternion
        Vector3 rotatedPoint = rotation * point;

        return rotatedPoint;
    }


    public void rotationToggled()
    {
        if (rotationActive)
        {
            rotationActive = false;
        }
        else
        {
            rotationActive = true;
        }
    }

    public void translationToggled()
    {
        if (translationActive)
        {
            translationActive = false;
        }
        else
        {
            translationActive = true;
        }
    }

    public void bendingToggled()
    {
        if (bendingActive)
        {
            bendingActive = false;
        }
        else
        {
            bendingActive = true;
        }
    }

    public void curvatureToggled()
    {
        if (curvingActive)
        {
            curvingActive = false;
        }
        else
        {
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

    public float GetRotationMultiplier() { return rotationGain; }
    public float GetTranslationMultiplier() { return translationGain; }
    public float GetBendingMultiplier() { return bendingGain; }
    public float GetCurvatureMultiplier() { return curvatureGain; }

    public async SendFirebaseData(float x_coordinate, float real_x_coordinates, float z_coordinates, float real_z_coordinates, float rotation, float real_rotation)
    {
        var data = new Dictionary<string, float>()
        {
            {"x_coordinate:", xCoordinate},
            {"real_x_coordinate", realXCoordinate},
            {"z_coordinate:", z_coordinate},
            {"real_z_coordinate:", real_z_coordinate},
            {"rotation:", rotation},
            {"real_rotation:", real_rotation},
        };

        try
        {
            await db.Collection("DATA").AddAsync(data);
            Debug.Log("Data sent to Firebase!");
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

}
