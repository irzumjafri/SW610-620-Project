using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class toggleMenu : MonoBehaviour
{
    public GameObject menuWindow;
    public GameObject referenceCamera;
    public InputActionAsset inputActions;

    private bool menuActive = false;
    private InputAction _menu;

    // Start is called before the first frame update
    void Start()
    {
        _menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        _menu.Enable();
        _menu.performed += menuPressed;
    }

    // Update is called once per frame
    public void menuPressed(InputAction.CallbackContext context)
    {
        
        if (menuActive)
        {
            menuWindow.SetActive(false);
            menuActive = false;
        }
        else
        {
            Vector3 menuLocation = referenceCamera.transform.position + referenceCamera.transform.forward * 2f;
            menuLocation.y = referenceCamera.transform.position.y;
            menuWindow.SetActive(true);
            menuWindow.transform.position = menuLocation;

            // Set menu rotation to point towards the pole
            Vector3 direction = new Vector3(referenceCamera.transform.position.x - menuWindow.transform.position.x, 0f, referenceCamera.transform.position.z - menuWindow.transform.position.z);
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
                menuWindow.transform.rotation = rotation;
            }

            menuActive = true;
        }
    }
}
