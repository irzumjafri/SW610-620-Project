using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

public class Select_TestSequence : MonoBehaviour
{
    public Point_generator pointGenerator;
    public RedirectionManager redirectionManager;
    public ToggleGains gainToggler;

    public Toggle translationGain;
    public Toggle curvatureToggle;
    public Toggle rotationToggle;
    public Toggle bendingToggle;
    public Toggle bendingSToggle;
    public Toggle randomToggle;

    public Toggle autoGainToggle;

    public Button resetButton;
    public Button teleportButton;

    public void ResetSequence()
    {
        // Deactivate all toggles
        curvatureToggle.isOn = false;
        rotationToggle.isOn = false;
        bendingToggle.isOn = false;
        bendingSToggle.isOn = false;
        randomToggle.isOn = false;

        if(autoGainToggle.isOn)
        {
            gainToggler.SetCurvature(false);
            //gainToggler.setTranslation(false);
            gainToggler.SetRotation(false);
            gainToggler.SetBending(false);
        }

        // Call Point_generator method to reset the sequence
        pointGenerator.ResetSequence();
    }

    void Start()
    {
        resetButton.onClick.AddListener(ResetSequence);
        teleportButton.onClick.AddListener(TeleportPlayer);
        // Add listener functions to handle toggle changes
        curvatureToggle.onValueChanged.AddListener(delegate { OnToggleChanged(curvatureToggle); });
        rotationToggle.onValueChanged.AddListener(delegate { OnToggleChanged(rotationToggle); });
        bendingToggle.onValueChanged.AddListener(delegate { OnToggleChanged(bendingToggle); });
        bendingSToggle.onValueChanged.AddListener(delegate { OnToggleChanged(bendingSToggle); });
        randomToggle.onValueChanged.AddListener(delegate { OnToggleChanged(randomToggle); });
    }

    // Method to handle toggle changes
    private void OnToggleChanged(Toggle changedToggle)
    {
        // Only allow one toggle to be active at a time
        if (changedToggle.isOn)
        {
            Toggle[] allToggles = { curvatureToggle, rotationToggle, bendingToggle, bendingSToggle, randomToggle };

            foreach (Toggle toggle in allToggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false;
                }
            }

            bool curvatureTest = curvatureToggle.isOn;
            bool rotationTest = rotationToggle.isOn;
            bool bendingTest = bendingToggle.isOn;
            bool bendingSTest = bendingSToggle.isOn;
            bool randomTest = randomToggle.isOn;

            if(autoGainToggle.isOn)
            {
                gainToggler.SetCurvature(curvatureTest);
                //gainToggler.setTranslation(translationtest);
                gainToggler.SetRotation(rotationTest);
                gainToggler.SetBending(bendingTest || bendingSTest);

            }
            


            pointGenerator.ResetSequence();

            // Call Point_generator method to initialize the selected test
            pointGenerator.InitializeTest(curvatureTest, rotationTest, bendingTest, bendingSTest, randomTest);
        }
        else
        {
            ResetSequence();
        }
        
    }

    public void TeleportPlayer()
    {
        if (translationGain.isOn)
        {
            translationGain.isOn = false; // Toggle off
            pointGenerator.TeleportToStart();
            redirectionManager.UpdatePreviousPosition();
            translationGain.isOn = true; // Toggle on
        }
        else
        {
            pointGenerator.TeleportToStart();
        }
    }

    
}
