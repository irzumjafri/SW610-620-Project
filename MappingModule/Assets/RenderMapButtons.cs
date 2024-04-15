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
        foreach (var map in MapManager.Instance.GetMaps())
        {
            // check if it is already there
            if(mButtons.Select(b => b.name).Contains(map.Name)){
                continue;
            }
            GameObject o = Instantiate(ButtonPrefab, transform);
            o.name = map.Name;
            mButtons.Add(o);
        }
    }
}
