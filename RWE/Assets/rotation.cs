using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;




public class rotation : MonoBehaviour
{

    public GameObject gameobject;
    public GameObject gazeinteractor;

    private float gain;
    private float change;
    private float previousXRotation;
    private float currentXRotation;
    // Start is called before the first frame update
    void Start()
    {
        previousXRotation = gazeinteractor.transform.rotation.eulerAngles.y;
        gain = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //gameobject.transform.RotateAround(gazeinteractor.transform.position, Vector3.up, 25*Time.deltaTime);
        Quaternion playerRotation = gazeinteractor.transform.rotation;
        currentXRotation = playerRotation.eulerAngles.y;

        change = currentXRotation - previousXRotation;

        gameobject.transform.RotateAround(gazeinteractor.transform.position, Vector3.up, gain * change);

        playerRotation = gazeinteractor.transform.rotation;
        currentXRotation = playerRotation.eulerAngles.y;

        previousXRotation = currentXRotation;
    }

    public void setMultiplier(float multiplier)
    {
        gain = multiplier;
    }
}
