using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

//
public class updateGains : MonoBehaviour
{

    public GameObject redirector;

    public Slider rotSlider;
    public TextMeshProUGUI rotValue;

    public Slider transSlider;
    public TextMeshProUGUI transValue;

    public Slider curvSlider;
    public TextMeshProUGUI curvValue;

    public Slider bendSlider;
    public TextMeshProUGUI bendValue;

    // Start is called before the first frame update
    void Start()
    {
        //set sliders to values redirectionManager defaults
        rotSlider.value = redirector.GetComponent<RedirectionManager>().GetRotationMultiplier();
        transSlider.value = redirector.GetComponent<RedirectionManager>().GetTranslationMultiplier();
        curvSlider.value = redirector.GetComponent <RedirectionManager>().GetCurvatureMultiplier();
        bendSlider.value = redirector.GetComponent<RedirectionManager>().GetBendingMultiplier();

        //set text boxes to the same values;
        rotValue.text = rotSlider.value.ToString();
        transValue.text = transSlider.value.ToString();
        curvValue.text = curvSlider.value.ToString();
        bendValue.text = bendSlider.value.ToString();
    }
    public void rotValueChanged()
    {
        redirector.GetComponent<RedirectionManager>().setRotationMultiplier(rotSlider.value);
        rotValue.text = rotSlider.value.ToString();
    }
    public void transValueChanged()
    {
        redirector.GetComponent<RedirectionManager>().setTranslationMultiplier(transSlider.value);
        transValue.text = transSlider.value.ToString();
    }
    public void curvValueChanged()
    {
        redirector.GetComponent<RedirectionManager>().setCurvatureMultiplier(curvSlider.value);
        curvValue.text = curvSlider.value.ToString();
    }
    public void bendValueChanged()
    {
        redirector.GetComponent<RedirectionManager>().setBendingMultiplier(bendSlider.value);
        bendValue.text = bendSlider.value.ToString();
    }
}
