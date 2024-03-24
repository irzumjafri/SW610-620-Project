using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class RedirectionManager : MonoBehaviour
{
    public GameObject cameraOffset;
    public GameObject mainCamera;

    private float rotationGain;
    private float translationGain;
    private float bendingGain;
    private float curvatureGain;

    private float previousXRotation;
    private Vector3 previousPosition;


    private bool rotationActive;
    private bool translationActive;
    private bool bendingActive;
    //private bool curvingActive;
    // Start is called before the first frame update
    void Start()
    {
        rotationActive = false;
        translationActive = false;
        bendingActive = false;
        //curvingActive = false;

        previousXRotation = mainCamera.transform.rotation.eulerAngles.y;
        previousPosition = mainCamera.transform.position;
        rotationGain = -0.5f;
        translationGain = 5;
        bendingGain = -0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion playerRotation = mainCamera.transform.rotation;
        float currentXRotation = playerRotation.eulerAngles.y;
        float change = currentXRotation - previousXRotation;
        if (Mathf.Abs(change) > 180f)
        {
            change -= Mathf.Sign(change) * 360f;
        }

        //Rotation
        if (rotationActive) {
            cameraOffset.transform.RotateAround(mainCamera.transform.position, Vector3.up, rotationGain * change);
        }

        //Translation
        Vector3 currentPosition = mainCamera.transform.position;
        
        if (translationActive) {
            Vector3 positionChange = translationGain*(currentPosition - previousPosition);
            cameraOffset.transform.Translate(new Vector3(positionChange.x,0,positionChange.z),Space.World);
        }

        //Bending
        if (bendingActive) {
            Vector3 positionChange = currentPosition - previousPosition;
            cameraOffset.transform.RotateAround(mainCamera.transform.position,Vector3.up,bendingGain*positionChange.magnitude);
        }

        previousXRotation = currentXRotation;
        previousPosition = mainCamera.transform.position;

        //Curvature        
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

    /*public void curvatureToggled()
    {
        if(curvingActive)
        {
            curvingActive = false;
        } else {
            curvingActive = true;
        }
    }*/
    
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
