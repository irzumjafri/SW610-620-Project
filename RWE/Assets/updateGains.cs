using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class updateGains : MonoBehaviour
{

    public GameObject redirector;
    public Slider slider;

    // private _redirector;
    // Start is called before the first frame update
    void Start()
    {
       //_rotation = redirector.GetComponent<Rotation>;
    }
    public void valueChanged()
    {
       redirector.GetComponent<rotation>().setMultiplier(slider.value);
       UnityEngine.Debug.Log(slider.value);
    }
}
