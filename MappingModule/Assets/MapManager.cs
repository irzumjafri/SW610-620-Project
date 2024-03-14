using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;

class MapManager : Singleton
{
    List<Map> maps;

    void LoadMaps()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "maps.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            maps = JsonUtility.FromJson<List<Map>>(json);
            Debug.Log(maps);
        }

    }

    // serialize maps into json
    // use Application.persistentDataPath + maps.data or 
    void SaveMaps()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "maps.json");
        string json = JsonUtility.ToJson(maps, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Maps saved to: " + filePath);
    }
}
