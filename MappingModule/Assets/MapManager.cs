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

    void Start(){
        LoadMaps();
    }

    void CreateMap(string name, List<Vector2> points){
        // normalize coordinates 
        List<Vector2> normalizedPoints = new List<Vector2>();
        // assume first is at (0, 0) and second is at (distance between(first,second), 0)
        normalizedPoints.Add(new Vector2(0, 0));
        for(int i = 1; i < points.Count; i++){
            normalizedPoints.Add(points[i] - points[0]);
        }
        double angle = Math.Atan(normalizedPoints[1].y/normalizedPoints[1].x);
        for(int i = 1; i < points.Count; i++){
            double cur_angle = Math.Atan(normalizedPoints[i].y/normalizedPoints[i].x);
            double target_angle = cur_angle - angle;
            normalizedPoints[i] = new Vector2(Math.Cos(target_angle), Math.Sin(target_angle)) * normalizedPoints[i].magnitude;
        }
        maps.Add(new Map(name, normalizedPoints));
    }

    Map GetMap(string name){
        foreach(var map : maps){
            if(map.Name == name){
                return map;
            }
        }
        return null;
    }
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
