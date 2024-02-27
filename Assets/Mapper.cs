using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(PlayerInput))]
public class Mapper : MonoBehaviour
{
    public XRRayInteractor interact;
    public GameObject wallPrefab;
    public GameObject start;
    public GameObject end;

    private GameObject wall;
    private bool _setMarker;
    private bool _creating = false;
    public void OnSetMarker()
    {
        _setMarker = true;
    }
    void Update()
    {
        Vector3 pos;
        interact.TryGetHitInfo(out pos, out _, out _, out _);
        end.transform.position = new Vector3(pos.x, end.transform.localScale.y/2, pos.z);
        if(!_creating){
            start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2, pos.z);
        }
        if (_setMarker)
        {
            _setMarker = false;
            wall = Instantiate(wallPrefab, start.transform.position, Quaternion.identity);
            _creating = true;
            start.transform.position = new Vector3(pos.x, start.transform.localScale.y/2, pos.z);


        }
        if(_creating){
            start.transform.LookAt(end.transform.position);
            end.transform.LookAt(start.transform.position);
            float distance = Vector3.Distance(start.transform.position, end.transform.position);
            wall.transform.position = start.transform.position + start.transform.forward * (distance / 2);
            wall.transform.rotation = start.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
		
            // Luo spatial ankkurin seinään. 
            wall.AddComponent<ARAnchor>();
        }
    }
}