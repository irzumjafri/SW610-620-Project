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
    public GameObject hand;
    public GameObject floor;

    private GameObject wall;
    private bool _settingWalling;
    private bool _setMarker;
    private bool _creating = false;
    private bool _settingFloorLevel = false;
    private List<GameObject> walls = new List<GameObject>();

    private bool _floor_setted = false;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);

        startButtonText.text = "Start flooring";
        
        start.SetActive(false);
        end.SetActive(false);
    }

    void OnStartButtonClick()
    {
        if(!_floor_setted){
            _settingFloorLevel = true;
            startButtonText.text = "End flooring";
            _floor_setted = true;
        } else if(_settingFloorLevel){

            floor.AddComponent<ARAnchor>();
            _settingFloorLevel = false;
            
            startButtonText.text = "end walling";
            //startButton.interactable = false;
            _settingWalling = true;

            start.SetActive(true);
            end.SetActive(true);
        } else if(_settingWalling) {
            _settingWalling = false;
            start.SetActive(false);
            end.SetActive(false);
            startButtonText.text = "ended walling";
        }
    }

    public void OnSetMarker()
    {
        if(_settingFloorLevel){
            floor.transform.position = hand.transform.position;
        }
        if(_settingWalling){
            _setMarker = true;  
        }
    }

    void Update()
    {
        if(_settingWalling){
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
                    walls.Add(wall);
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

            }
        }
    }
}