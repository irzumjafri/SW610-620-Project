using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Mapper : MonoBehaviour
{
    public XRRayInteractor interact;
    public GameObject _gameObject;
    private GameObject prevGameObject;
    private bool _setMarker;
    public void OnSetMarker()
    {
        _setMarker = true;
    }
    void Update()
    {
        Vector3 pos;
        interact.TryGetHitInfo(out pos, out _, out _, out _);
        _gameObject.transform.position = pos;
        if (_setMarker)
        {
            _setMarker = false;
            if (prevGameObject != null)
                Debug.DrawLine(prevGameObject.transform.position, pos);
            prevGameObject = _gameObject;
            _gameObject = Instantiate(prevGameObject);
        }
    }
}