using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Translation : MonoBehaviour
{

    public GameObject gameobject;
    public GameObject gazeinteractor;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 positionChange;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = gazeinteractor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = gazeinteractor.transform.position;
        positionChange = 5*(currentPosition -previousPosition);
        gameobject.transform.Translate(new Vector3(positionChange.x,0,positionChange.z),Space.World);
        previousPosition = gazeinteractor.transform.position;
    }
}