using UnityEngine;

public class rotation : MonoBehaviour
{
    public GameObject gameobject;
    public GameObject gazeinteractor;

    private float gain;
    private float previousXRotation;

    // Start is called before the first frame update
    void Start()
    {
        previousXRotation = gazeinteractor.transform.rotation.eulerAngles.y;
        gain = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion playerRotation = gazeinteractor.transform.rotation;
        float currentXRotation = playerRotation.eulerAngles.y;

        // Calculate the change in rotation
        float change = currentXRotation - previousXRotation;

        // Adjust for full rotations
        if (Mathf.Abs(change) > 180f)
        {
            change -= Mathf.Sign(change) * 360f;
        }

        // Apply the rotation to the gameobject
        gameobject.transform.RotateAround(gazeinteractor.transform.position, Vector3.up, gain * change);

        // Update previous rotation
        previousXRotation = currentXRotation;
    }

    public void setMultiplier(float multiplier)
    {
        gain = multiplier;
    }
}
