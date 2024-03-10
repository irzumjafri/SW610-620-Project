using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;

class MapManager : Singleton {
    List<Map> maps;
    
    void LoadMaps(){

    }

    // serialize maps into json
    // use Application.persistentDataPath + maps.data or 
    void SaveMaps(){

    }
}