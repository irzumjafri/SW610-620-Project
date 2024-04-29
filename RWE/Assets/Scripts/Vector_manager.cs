using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector_manager : MonoBehaviour
{
    public static Vector3[] Curvature_points = new Vector3[]
    {
        // Long straight walks to demo the curvature 
        new Vector3(5, 0, 0),
        new Vector3(10, 0, 0),
        new Vector3(10, 0, 2),
        new Vector3(8, 0, 2),
        new Vector3(10, 0, 10),
        new Vector3(10, 0, 15)
    };

    public static Vector3[] Rotation_points = new Vector3[]
    {
        // Purpose is to have small distances and a lot of rotations
        new Vector3(4, 0, 0),
        new Vector3(4, 0, 4),
        new Vector3(0, 0, 4),
        new Vector3(0, 0, 0),
        new Vector3(4, 0, -4),
        new Vector3(4, 0, -4),
        new Vector3(0, 0, 0)
    };

    public static Vector3[] Bending_points = new Vector3[]
        {
        // Purpose is to hvae a circular path  
        new Vector3(5.6f, 0f, 0f),
        new Vector3(3.962f, 0f, 3.962f),
        new Vector3(0f, 0f, 5.6f),
        new Vector3(-3.962f, 0f, 3.962f),
        new Vector3(-5.6f, 0f, 0f),
        new Vector3(-3.962f, 0f, -3.962f),
        new Vector3(0f, 0f, -5.6f),
        new Vector3(3.962f, 0f, -3.962f)
    };

    public static void ResetState()
    {
        // Reset Curvature_points
        Curvature_points = new Vector3[]
        {
            new Vector3(5, 0, 0),
            new Vector3(10, 0, 0),
            new Vector3(10, 0, 2),
            new Vector3(8, 0, 2),
            new Vector3(10, 0, 10),
            new Vector3(10, 0, 15)
        };

        // Reset Rotation_points
        Rotation_points = new Vector3[]
        {
            new Vector3(4, 0, 0),
            new Vector3(4, 0, 4),
            new Vector3(0, 0, 4),
            new Vector3(0, 0, 0),
            new Vector3(4, 0, -4),
            new Vector3(4, 0, -4),
            new Vector3(0, 0, 0)
        };

        // Reset Bending_points
        Bending_points = new Vector3[]
        {
            new Vector3(5.6f, 0f, 0f),
            new Vector3(3.962f, 0f, 3.962f),
            new Vector3(0f, 0f, 5.6f),
            new Vector3(-3.962f, 0f, 3.962f),
            new Vector3(-5.6f, 0f, 0f),
            new Vector3(-3.962f, 0f, -3.962f),
            new Vector3(0f, 0f, -5.6f),
            new Vector3(3.962f, 0f, -3.962f)
        };
    }
}


