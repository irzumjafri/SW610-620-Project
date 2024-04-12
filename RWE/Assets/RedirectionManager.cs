
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
    [Range(-5f, 5f)]
    private float curvatureGain;
    [SerializeField]
    [Tooltip("Default multiplier value for Bending gain")]
    [Range(-5f, 5f)]
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
        updateRealPosition();

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

        UpdatePreviousPosition();


        // Save the position of cameraOffset to a file
        SavePositionToFile();
    }

    public void updateRealPosition() {
        previousRealRotation += (mainCamera.transform.rotation.eulerAngles.y - previousXRotation);
        previousRealRotation = previousRealRotation % 360;
        previousRealPosition += (mainCamera.transform.position - previousPosition);
    }

    public void UpdatePreviousPosition()
    {
        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
    }

    private void setUpSaveToFile(){
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
        Vector3 currentPosition = mainCamera.transform.position;
        Vector3 previousVector = previousPosition - previous2Position;
        Vector3 currentVector = currentPosition- previousPosition;
        // Get the user's head rotation from the VR headset
        Quaternion headRotation = Camera.main.transform.rotation;

        // Calculate the bending gain effect based on the user's head rotation
        float bend = Mathf.DeltaAngle(currentXRotation, previousXRotation);

        //float previousAngle = Vector3.Angle(previousVector, new Vector3(1,0,0));
        //float bendAmount = Mathf.DeltaAngle(Vector3.Angle(previousVector, new Vector3(1,0,0)), Vector3.Angle(currentVector, new Vector3(1,0,0))) * bendingGain;

        Vector3 positionChange = currentPosition - previousPosition;
        Debug.Log("angle: " + bend.ToString("F10"));

        if (bend < 0) {
            cameraOffset.transform.RotateAround(mainCamera.transform.position,Vector3.up,-bendingGain*positionChange.magnitude);
        } else if (bend > 0) {
            cameraOffset.transform.RotateAround(mainCamera.transform.position,Vector3.up,bendingGain*positionChange.magnitude);
        }
        

        



        /*
        //Debug.Log(Vector3.Angle(previousVector, new Vector3(0,0,1)));
        //Debug.Log("R: " + currentXRotation);
        Debug.Log("angle: " + bend.ToString("F10"));

        Quaternion rotation = Quaternion.Euler(0,previousAngle+bend,0);

        Vector3 change = rotation * new Vector3(0,0,1) * currentVector.magnitude;
        //previousVector.normalized * currentVector.magnitude;

        cameraOffset.transform.Translate(change,Space.World);

        */

        // Adjust the curvature of the environment based on the bend amount
        //Vector3 newRotation = environment.rotation.eulerAngles;
        //newRotation.z = bendAmount; // Adjust the Z rotation to create curvature
        //environment.rotation = Quaternion.Euler(newRotation);


        /*Vector3 currentPosition = mainCamera.transform.position;
        Vector3 previousVector = previousPosition - previous2Position;
        Debug.Log("prevV: " + previousVector.ToString("F10"));

        Vector3 currentVector = currentPosition- previousPosition;
        Debug.Log("curV: " + currentVector.ToString("F10"));

        Vector3 prev_n = Vector3.Normalize(previousVector);
        Vector3 cur_n = Vector3.Normalize(currentVector);

        Quaternion rotation = Quaternion.Euler(0,0,0);
        Vector3 change = rotation * previousVector;
        cameraOffset.transform.Translate(change,Space.World);


        // Check if vectors are not zero to avoid division by zero
        if (previousVector != Vector3.zero && currentVector != Vector3.zero)
        {

            // Calculate the angle between the vectors in degrees
            //float angle = (float)(Vector3.Angle(previousVector, currentVector));
            float angle = Mathf.Acos(Vector3.Dot(previousVector.normalized, currentVector.normalized)) * Mathf.Rad2Deg;

            // Double the angle
            float doubledAngle = angle * bendingGain;

            //Debug.Log("angle: " + angle);

            

            // Calculate the cross product of the vectors
            Vector3 axis = Vector3.Cross(previousVector, currentVector);

            // Determine the direction of rotation
            float direction = Mathf.Sign(axis.y);

            //Debug.Log("Sign: " + direction);

            // Determine the direction of rotation
            /*if (direction < 0)
            {
                doubledAngle = -doubledAngle;
            }
            //Debug.Log("Doupleangle: " + doubledAngle);

            if(angle > Mathf.Epsilon){

                float prevangle = (float)Math.Atan(previousVector.x/previousVector.z);

                //Quaternion rotation = Quaternion.Euler(0,30,0);

                // Rotate the point using the quaternion
                
                //previousVector.normalized * currentVector.magnitude;
                //rotation * (5*currentVector.magnitude * 

                //cameraOffset.transform.Translate(currentVector.magnitude*(float)Math.Cos((doubledAngle+prevangle)*Mathf.Deg2Rad),0,currentVector.magnitude*(float)Math.Sin((doubledAngle+prevangle)*Mathf.Deg2Rad),Space.World);

                //Debug.Log("PREV: " + previousVector.x + previousVector.z);

                //Debug.Log("cur: " + currentVector.x + currentVector.z);

                //cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, doubledAngle);
                //Vector3 change = Quaternion.AngleAxis(doubledAngle, Vector3.up) * currentVector;
                //RotatePointAroundOrigin(currentVector, doubledAngle);
                //Debug.Log("c: " + change.x + change.z);

            
                
            }
            
        }*/
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

    public float GetRotationMultiplier() { return rotationGain; }
    public float GetTranslationMultiplier() {  return translationGain; }
    public float GetBendingMultiplier() {  return bendingGain; }
    public float GetCurvatureMultiplier() {  return curvatureGain; }

}
