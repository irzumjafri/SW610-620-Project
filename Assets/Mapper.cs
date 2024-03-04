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
    public Button resetButton;
    public TextMeshProUGUI startButtonText;
    public GameObject hand;
    public GameObject floor;

    private GameObject wall;
    private bool _settingWalling = false;
    private bool _setMarker = false;
    private bool _creating = false;
    private bool _settingFloorLevel = false;
    private List<GameObject> walls = new List<GameObject>();

    private bool _floor_setted = false;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        resetButton.onClick.AddListener(RestartMapping);

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

            _settingFloorLevel = false;
            
            startButtonText.text = "end walling";
            //startButton.interactable = false;
            _settingWalling = true;

            start.SetActive(true);
            end.SetActive(true);
        } else if(_settingWalling) {
            _settingWalling = false;
            Destroy(walls[walls.Count - 1]);
            start.SetActive(false);
            end.SetActive(false);
            startButtonText.text = "walling finished";
        } else {

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

    void RestartMapping(){
        foreach(GameObject wall in walls){
            Destroy(wall);
        }
        walls.Clear();

        _settingWalling = false;
        _setMarker = false;
        _creating = false;
        _settingFloorLevel = false;
        _floor_setted = false;

        startButtonText.text = "Start flooring";
    }

    void Update()
    {
        if(_settingWalling){
            Vector3 pos;
            interact.TryGetHitInfo(out pos, out _, out _, out _);
            end.transform.position = new Vector3(pos.x, end.transform.localScale.y/2+floor.transform.position.y, pos.z);
            if(!_creating){
                start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2+floor.transform.position.y, pos.z);
            }
            if (_setMarker)
            {
                _setMarker = false;
                if(wall != null){
                    //wall.AddComponent<ARAnchor>();
                }
                
                wall = Instantiate(wallPrefab, start.transform.position, Quaternion.identity);
                walls.Add(wall);
                _creating = true;
                start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2+floor.transform.position.y, pos.z);
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