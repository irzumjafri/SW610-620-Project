using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGains : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle translationToggle;
    public Toggle rotationToggle;
    public Toggle curvatureToggle;
    public Toggle bendingToggle;

    public GameObject redirector;
    private RedirectionManager redirectionManager;
    
    void Start()
    {
        redirectionManager = redirector.GetComponent<RedirectionManager>();
        curvatureToggle.onValueChanged.AddListener(delegate { redirectionManager.SetCurvatureActive(curvatureToggle.isOn); ; });
        rotationToggle.onValueChanged.AddListener(delegate { redirectionManager.SetRotationActive(rotationToggle.isOn); ; });
        bendingToggle.onValueChanged.AddListener(delegate { redirectionManager.SetBendingActive(bendingToggle.isOn); ; });
        translationToggle.onValueChanged.AddListener(delegate { redirectionManager.SetTranslationActive(translationToggle.isOn); ; });
    }

    // Update is called once per frame
    public void SetRotation(bool state)
    {
        rotationToggle.isOn = state;
        redirectionManager.SetRotationActive(state);
    }

    public void SetTranslation(bool state)
    {  
        translationToggle.isOn = state;
        redirectionManager.SetTranslationActive(state);
    }

    public void SetCurvature(bool state)
    {
        curvatureToggle.isOn = state;
        redirectionManager.SetCurvatureActive(state);
    }

    public void SetBending(bool state)
    {
        bendingToggle.isOn = state;
        redirectionManager.SetBendingActive(state);
    }
}
