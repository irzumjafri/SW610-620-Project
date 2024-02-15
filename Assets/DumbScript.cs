using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DumbScript : MonoBehaviour
{
    public XRRayInteractor interact;
    public XRController rightHand;
    public InputHelpers.Button button;
    public GameObject _gameObject;
    
    int i = 0;
    
    private GameObject prevGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        interact.TryGetHitInfo(out pos, out _, out _, out _);
        _gameObject.transform.position = pos;

        bool pressed = false;
        rightHand.inputDevice.IsPressed(button, out pressed);
        // this pretends a button press :D
        if(pressed){
Debug.Log("CAlled");
if(prevGameObject != null)
Debug.DrawLine(prevGameObject.transform.position, pos);
prevGameObject = _gameObject;
_gameObject = Instantiate(prevGameObject);

         

          
}
       
    }
}
