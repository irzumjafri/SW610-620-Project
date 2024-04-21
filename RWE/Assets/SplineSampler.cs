using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer m_splineContainer;
    [SerializeField]
    private int m_splineIndex;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_time;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_width;

    float3 position;
    float3 forward;
    float3 upVector;

    // Update is called once per frame
    void Update()
    {
        m_splineContainer.Evaluate(m_splineIndex, m_time, out position, out forward, out upVector);
        float3 right = Vector3.Cross(forward, upVector).normalized;
        //p1 = position + (right * m_width);
        //p2 = position + (-right * m_width);
    }

    private void OnDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0, position, Quaternion.identity, 1f, EventType.Repaint);
    }
}
