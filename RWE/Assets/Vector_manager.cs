using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector_manager : MonoBehaviour
{

    public static Vector3[] Curvature_points = new Vector3[]
    {
        new Vector3(10, 0, 0),
        new Vector3(10, 0, 10),
        new Vector3(20, 0, 10),
        new Vector3(20, 0, 20)
    };

    public static Vector3[] Rotation_points = new Vector3[]
    {
        new Vector3(5, 0, 0),
        new Vector3(5, 0, 5),
        new Vector3(0, 0, 5),
        new Vector3(0, 0, 0),
        new Vector3(5, 0, -5),
        new Vector3(5, 0, -5),
        new Vector3(0, 0, 0)
    };

    public static Vector3[] Bending_points = new Vector3[]
    {
        new Vector3(8f, 0f, 0f),
        new Vector3(5.66f, 0f, 5.66f),
        new Vector3(0f, 0f, 8f),
        new Vector3(-5.66f, 0f, 5.66f),
        new Vector3(-8f, 0f, 0f),
        new Vector3(-5.66f, 0f, -5.66f),
        new Vector3(0f, 0f, -8f),
        new Vector3(5.66f, 0f, -5.66f)
    };
}


