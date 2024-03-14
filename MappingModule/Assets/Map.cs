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
    public string anchor1;
    public string anchor2;
    constructor(string name, List<Vector> points, string anchor1, string anchor2){
        Name = name;
        Points = points;
        this.anchor1 = anchor1;
        this.anchor2 = anchor2;
    }
}