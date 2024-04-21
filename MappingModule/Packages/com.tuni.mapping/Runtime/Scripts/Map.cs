using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class Map
{
    [JsonProperty]
    public string Name;

    // points should be normalized
    [JsonProperty]
    private readonly List<SerializableVector2> _points = new List<SerializableVector2>();

    public List<Vector2> Points { get
        {
            return _points.Select(v => new Vector2(v.x, v.y)).ToList();
        }
        set
        {
            _points.Clear();
            foreach (var point in value)
            {
                _points.Add(new SerializableVector2(point));
            }
        }
    }

    [JsonProperty]
    public string anchor1;

    [JsonProperty]
    public string anchor2;
    public Map(string name, List<Vector2> _points, string anchor1, string anchor2){
        Name = name;
        this.anchor1 = anchor1;
        this.anchor2 = anchor2;
        this._points.AddRange(_points.Select(point => new SerializableVector2(point)));
    }

    public double Perimeter {
        get {
            double size = 0;
            var points = Points;
            for(int i = 0; i < points.Count - 1; i++){
                size += Vector2.Distance(points[i], points[i + 1]);
            }
            return size;
        }
    }
}