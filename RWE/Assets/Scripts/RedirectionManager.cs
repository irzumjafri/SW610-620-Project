
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using SystemVector3 = System.Numerics.Vector3;
using SystemQuaternion = System.Numerics.Quaternion;


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

    private float previousYRotation;
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
    public int firebaseDataCounter = 0;


    string usedFilePath;
    // Start is called before the first frame update
    void Start()
    {
        rotationActive = false;
        translationActive = false;
        bendingActive = false;
        curvingActive = false;

        //Initializing Database for Firebase
        var firebaseApp = FirebaseApp.DefaultInstance;
        db = FirebaseFirestore.GetInstance(firebaseApp);
        // db = FirebaseFirestore.DefaultInstance;

        previousYRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
        previousRealRotation = previousYRotation;
        previousRealPosition = previousPosition;

        setUpSaveToFile();
        SendSessionInfoToFirebase();
        InvokeRepeating("SavePositionToFile", 0.5F, 0.5F);
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
        // calculating real rotated angle
        float realAngle = mainCamera.transform.rotation.eulerAngles.y - previousYRotation;
        previousRealRotation += realAngle;
        previousRealRotation = previousRealRotation % 360;

        // Rotation compared to previous real rotation
        float totalRotation = mainCamera.transform.rotation.eulerAngles.y - previousRealRotation;

        // Angle to manupulate current wector into right direction
        float angleToBeManipulated2 = realAngle - totalRotation;

        // Updated position vector to be manipulated
        Vector3 currentVector = mainCamera.transform.position - previousPosition;

        // Rotating current vector to match real rotation
        if (currentVector.magnitude > 0 ) {
            Quaternion rotation = Quaternion.Euler(0f, angleToBeManipulated2 , 0f);
            Vector3 changeVector = rotation * currentVector;
            previousRealPosition += changeVector;
        }
    }


    public void UpdatePreviousPosition()
    {
        previousYRotation = mainCamera.transform.rotation.eulerAngles.y;
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
        try
        {
            // Open a file stream to write the positions
            using (StreamWriter writer = new StreamWriter(usedFilePath, append: true))
            {
                // Write the position to the file
                writer.WriteLine(previousPosition.x + ";" + previousRealPosition.x + ";" + previousPosition.y + ";" + previousRealPosition.y + ";" + previousPosition.z + ";" + previousRealPosition.z + ";" + previousYRotation + ";" + previousRealRotation);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving position to file: " + e.Message);
        }
        
        SendFirebaseData(previousPosition.x, previousRealPosition.x, previousPosition.z, previousRealPosition.z, previousYRotation, previousRealRotation);
    }

    private void injectRotation()
    {
        float currentYRotation = mainCamera.transform.rotation.eulerAngles.y;

        // Calculate the change in rotation
        float change = currentYRotation - previousYRotation;

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

        // Calculating the change in position
        Vector3 positionChange = translationGain * (currentPosition - previousPosition);

        // Apply the translation to the gameobject
        cameraOffset.transform.Translate(new Vector3(positionChange.x, 0, positionChange.z), Space.World);
    }

    private void injectCurvature()
    {
        Vector3 currentPosition = mainCamera.transform.position;

        // Calculating the change in position
        Vector3 positionChange = currentPosition - previousPosition;

        // Applying curvature to the game object
        cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, curvatureGain * positionChange.magnitude);
    }

    private void injectBending()
    {
        float currentYRotation = mainCamera.transform.rotation.eulerAngles.y;
        Vector3 currentPosition = mainCamera.transform.position;

        // Calculating the change in position
        Vector3 positionChange = currentPosition - previousPosition;

        // Calculate the bending gain effect based on the user's head rotation
        float bend = Mathf.DeltaAngle(currentYRotation, previousYRotation);

        // Applying bending to the gameobject
        cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, bendingGain * positionChange.magnitude * bend);
      
    }

    public void SetRotationActive(bool state)
    {
        rotationActive = state;
    }

    public void SetCurvatureActive(bool state)
    {
        curvingActive = state;
    }

    public void SetTranslationActive(bool state)
    {
        translationActive = state;
    }

    public void SetBendingActive(bool state)
    {
        bendingActive = state;
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

    public async Task SendFirebaseData(float x_coordinate, float real_x_coordinate, float z_coordinate, float real_z_coordinate, float rotation, float real_rotation)
    {
        firebaseDataCounter++;
        var data = new Dictionary<string, object>()
        {
            {"timestamp", DateTime.Now},
            {"x_coordinate", x_coordinate},
            {"real_x_coordinate", real_x_coordinate},
            {"z_coordinate", z_coordinate},
            {"real_z_coordinate", real_z_coordinate},
            {"rotation", rotation},
            {"real_rotation", real_rotation},
        };
        Debug.Log(x_coordinate);
        try
        {
            String sessionID = AnalyticsSessionInfo.sessionId.ToString();
            await db.Collection("LoggedData").Document(sessionID).Collection("Data").Document(firebaseDataCounter.ToString()).SetAsync(data);
            //Debug.Log("Data sent to Firebase!");
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    public async Task SendSessionInfoToFirebase()
    {
        var sessionData = new Dictionary<string, object>()
            {
                { "date", DateTime.Now},
                { "test_sequence", "demo"}
            };
        try
        {
            String sessionID = AnalyticsSessionInfo.sessionId.ToString();
            await db.Collection("LoggedData").Document(sessionID).SetAsync(sessionData);
            Debug.Log("Session info sent to firebase");
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

}
