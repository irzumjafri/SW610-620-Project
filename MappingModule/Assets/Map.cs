using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
class Map {
    public string Name;
    
    // points should be normalized
    public List<Vector2> Points;
    constructor(string name, List<Vector> points){
        Name = name;
        Points = points;
    }
}