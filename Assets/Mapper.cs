using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
public class Mapper : MonoBehaviour
{
    public XRRayInteractor interact;
    public GameObject wallPrefab;
    public GameObject start;
    public GameObject end;
    public Button startButton;
    public TextMeshProUGUI startButtonText;

    private GameObject wall;
    private bool _started;
    private bool _setMarker;
    private bool _creating = false;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);

        start.SetActive(false);
        end.SetActive(false);
    }

    void OnStartButtonClick()
    {
        _started = true;

        startButtonText.text = "Mapping started";
        startButton.interactable = false;

        start.SetActive(true);
        end.SetActive(true);
    }

    public void OnSetMarker()
    {
        _setMarker = true;
    }

    void Update()
    {
        if(_started){
            Vector3 pos;
            interact.TryGetHitInfo(out pos, out _, out _, out _);
            end.transform.position = new Vector3(pos.x, end.transform.localScale.y/2, pos.z);
            if(!_creating){
                start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2, pos.z);
            }
            if (_setMarker)
            {
                _setMarker = false;
                if(wall != null){
                    wall.AddComponent<ARAnchor>();
                }
                
                wall = Instantiate(wallPrefab, start.transform.position, Quaternion.identity);
                _creating = true;
                start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2, pos.z);
            }
            if(_creating)
            {
                start.transform.LookAt(end.transform.position);
                end.transform.LookAt(start.transform.position);
                float distance = Vector3.Distance(start.transform.position, end.transform.position);
                Vector3 wallPosition = start.transform.position + start.transform.forward * (distance / 2);
                Quaternion wallRotation = start.transform.rotation;

                // Apply transformations to the wall
                wall.transform.position = wallPosition;
                wall.transform.rotation = wallRotation;
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);

                /*
                // Check if ARAnchor component already exists
                ARAnchor arAnchor = wall.GetComponent<ARAnchor>();
                if (arAnchor == null)
                {
                    // Add ARAnchor component to the wall
                    arAnchor = wall.AddComponent<ARAnchor>();
                }

                // Ensure ARAnchor is updated with the current wall position
                arAnchor.transform.position = wallPosition;
                arAnchor.transform.rotation = wallRotation;
                */
            }
        }
    }
}