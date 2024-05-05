using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RenderMapButtons : MonoBehaviour
{
    public GameObject ButtonPrefab;

    private List<GameObject> mButtons = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        // spawn as children
        var maps = MapManager.Instance.GetMaps();
        foreach (var map in maps)
        {
            // check if it is already there
            if(mButtons.Select(b => b.name).Contains(map.Name)){
                continue;
            }
            GameObject o = Instantiate(ButtonPrefab, transform);
            o.name = map.Name;
            mButtons.Add(o);
        }
        // find maps which should be there
        
        List<GameObject> toRemove = new List<GameObject>();
        foreach(var map in mButtons)
        {
            if (!maps.Select(m => m.Name).Contains(map.name))
            {
                toRemove.Add(map);
            }
        }
        foreach(var map in toRemove)
        {
            mButtons.Remove(map);
            Destroy(map);
        }
    }
}
