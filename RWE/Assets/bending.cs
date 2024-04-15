using UnityEngine;

public class bending : MonoBehaviour
{
    public GameObject gameobject;
    public GameObject gazeinteractor;
    private float gain;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 positionChange;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = gazeinteractor.transform.position;
        gain = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion playerRotation = gazeinteractor.transform.rotation;
        float currentXRotation = playerRotation.eulerAngles.y;

        currentPosition = gazeinteractor.transform.position;

        positionChange = currentPosition - previousPosition;

        // Apply the rotation to the gameobject
        gameobject.transform.RotateAround(gazeinteractor.transform.position, Vector3.up, 50 * positionChange.magnitude);

        previousPosition = gazeinteractor.transform.position;
    }
}
