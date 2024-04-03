using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabManager : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    public GameObject tab1;
    public GameObject tab2;
    public GameObject tab3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Activate1()
    {
        tab1.SetActive(true);
        tab2.SetActive(false);
        tab3.SetActive(false);
    }
    public void Activate2()
    {
        tab1.SetActive(false);
        tab2.SetActive(true);
        tab3.SetActive(false);
    }
    public void Activate3()
    {
        tab1.SetActive(false);
        tab2.SetActive(false);
        tab3.SetActive(true);
    }
    
}
