using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapManager : Singleton<MapManager>
{
    private List<Map> maps = new List<Map>();

    void Start(){
        LoadMaps();
    }

    private float CalculateAngle(Vector2 vec)
    {
        if(vec.x == 0)
        {
            if(vec.y > 0)
            {
                return Mathf.PI / 2;
            }
            return 3 * Mathf.PI / 2;
        }
        float angle = Mathf.Atan(vec.y / vec.x);
        if(vec.x < 0)
        {
            angle += Mathf.PI;
        }
        return angle;
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
        float angle = CalculateAngle(normalizedPoints[1]);
        for(int i = 1; i < points.Count; i++){
            float cur_angle = CalculateAngle(normalizedPoints[i]);
            float target_angle = cur_angle - angle;
            normalizedPoints[i] = new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)) * normalizedPoints[i].magnitude;
        }
        maps.Add(new Map(name, normalizedPoints, anchor1, anchor2));
        if(save_map){
            SaveMaps();
        }
    }

    public void DeleteMap(string name)
    {
        Map map = GetMap(name);
        if (map != null)
        {
            maps.Remove(map);
            SaveMaps();
        }
    }

    // Returns unity coordinates that are calculated from normalized coordinates with known anchor positions
    public List<Vector2> GetUnityCoordinates(Map map, Vector2 anchor1, Vector2 anchor2){
        List<Vector2> normalizedPoints = new()
        {
            anchor1,
            anchor2
        };
        Vector2 diff = anchor2 - anchor1;
        float angle = CalculateAngle(diff);
        for(int i = 2; i < map.Points.Count; i++)
        {
            float cur_angle = CalculateAngle(map.Points[i]);
            float target_angle = cur_angle + angle;
            normalizedPoints.Add(anchor1 + new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)) * map.Points[i].magnitude);
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
        Debug.Log("Loading maps from: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            //  maps = JsonUtility.FromJson<List<Map>>(json);
            maps = JsonConvert.DeserializeObject<List<Map>>(json);
            Debug.Log(maps.Count);
        }

    }

    // serialize maps into json
    // use Application.persistentDataPath + maps.data or 
    void SaveMaps()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "maps.json");
        Debug.Log("Saving amount of maps:" + maps.Count);
        string json = JsonConvert.SerializeObject(maps);
        File.WriteAllText(filePath, json);
        Debug.Log("Maps saved to: " + filePath);
    }
}
