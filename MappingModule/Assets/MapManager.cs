using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

class MapManager : Singleton<MapManager>
{
    private List<Map> maps;

    void Start(){
        LoadMaps();
    }

    public void CreateMap(string name, List<Vector2> points, string anchor1, string anchor2, bool save_map = true){
        // normalize coordinates 
        List<Vector2> normalizedPoints = new()
        {
            // assume first is at (0, 0) and second is at (distance between(first,second), 0)
            new Vector2(0, 0)
        };
        for(int i = 1; i < points.Count; i++){
            normalizedPoints.Add(points[i] - points[0]);
        }
        float angle = Mathf.Atan(normalizedPoints[1].y/normalizedPoints[1].x);
        for(int i = 1; i < points.Count; i++){
            float cur_angle = Mathf.Atan(normalizedPoints[i].y/normalizedPoints[i].x);
            float target_angle = cur_angle - angle;
            normalizedPoints[i] = new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)) * normalizedPoints[i].magnitude;
        }
        maps.Add(new Map(name, normalizedPoints, anchor1, anchor2));
        if(save_map){
            SaveMaps();
        }
    }

    // Returns unity coordinates that are calculated from normalized coordinates with known anchor positions
    public List<Vector2> GetUnityCoordinates(Map map, Vector2 anchor1, Vector2 anchor2){
        List<Vector2> normalizedPoints = new List<Vector2>
        {
            anchor1,
            anchor2
        };
        float angle = Mathf.Atan(normalizedPoints[1].y/normalizedPoints[1].x);
        for(int i = 2; i < map.Points.Count; i++){
            float cur_angle = Mathf.Atan(normalizedPoints[i].y/normalizedPoints[i].x);
            float target_angle = cur_angle - angle;
            normalizedPoints[i] = anchor1 + new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)) * normalizedPoints[i].magnitude;
        }
        return normalizedPoints;
    }

    public Map GetMap(string name){
        foreach(var map in maps){
            if(map.Name == name){
                return map;
            }
        }
        return null;
    }

    public List<Map> GetMaps(){
        return maps;
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
