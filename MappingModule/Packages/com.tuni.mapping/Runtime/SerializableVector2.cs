using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableVector2
{
    public float x; public float y;

    public Vector2 AsUnityVector()
    {
        return new Vector2(x, y);
    }
    [JsonConstructor]
    public SerializableVector2(float x,float y)
    {
        this.x = x;
        this.y = y;
    }
    public SerializableVector2(Vector2 v)
    {
        x = v.x;
        y = v.y;
    }
}
