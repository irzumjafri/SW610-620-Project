using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


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
    // private _redirector;
    // Start is called before the first frame update
    void Start()
    {
        rotValue.text = rotSlider.value.ToString();
        transValue.text = transSlider.value.ToString();
        curvValue.text = curvSlider.value.ToString();
        bendValue.text = bendSlider.value.ToString();
        //_rotation = redirector.GetComponent<Rotation>;
    }
    public void rotValueChanged()
    {
        redirector.GetComponent<rotation>().setMultiplier(rotSlider.value);
        rotValue.text = rotSlider.value.ToString();
    }
    public void transValueChanged()
    {
        //redirector.GetComponent<rotation>().setMultiplier(transSlider.value);
        transValue.text = transSlider.value.ToString();
    }
    public void curvValueChanged()
    {
        //redirector.GetComponent<rotation>().setMultiplier(curvSlider.value);
        curvValue.text = curvSlider.value.ToString();
    }
    public void bendValueChanged()
    {
        //redirector.GetComponent<rotation>().setMultiplier(bensSlider.value);
        bendValue.text = bendSlider.value.ToString();
    }
}
