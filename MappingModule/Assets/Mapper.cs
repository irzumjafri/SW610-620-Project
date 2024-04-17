using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Linq;


public enum State
{
	Initial,
	LocatingAnchors,
	Floor, // At this state, user sets floor level
	Mapping, // At this state, walls can be added
	Finished
}
public class Mapper : Singleton<Mapper>
{
	public XRRayInteractor interact; // the rayinteractor used for raycasting in mapping
	public GameObject wallPrefab;
	public GameObject hand;
	public GameObject floor;
	private List<GameObject> walls = new List<GameObject>();
	public State state { get {
			return _state;
		} }
	private State _state = State.Initial;
	private bool mapLoaded = false;
	private Map map;
	private List<OVRSpatialAnchor> pillars = new List<OVRSpatialAnchor>();
	private List<OVRSpatialAnchor.UnboundAnchor> tempAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();

	public OVRSpatialAnchor pillarPrefab;
	public GameObject pillar;
	private GameObject wallToLookAt;
	

	async Task OnAllLocated(){
		if(tempAnchors.Count != 2) {
			return;
		}
		mapLoaded = true;


        if (tempAnchors[0].Uuid.ToString() != map.anchor1)
        {
            tempAnchors.Reverse();
        }

        foreach (var unboundAnchor in tempAnchors){
			pillars.Add(BindUnbound(unboundAnchor));
		}
		// we can delete temp anchors now
		tempAnchors.Clear();

        // now we need to draw all the other pillars too
        var points = MapManager.Instance.GetUnityCoordinates(map, V3ToV2(pillars[0].transform.position), V3ToV2(pillars[1].transform.position));

        for (int i = 2; i < points.Count; i++){
			OVRSpatialAnchor pillarCopy = Instantiate(pillarPrefab, NormalizePosition(new Vector3(points[i].x, 0, points[i].y)), Quaternion.identity);

            await AnchorCreated(pillarCopy);
		}
	}

	Vector2 V3ToV2(Vector3 vector){
		return new Vector2(vector.x, vector.z);
	}

	OVRSpatialAnchor BindUnbound(OVRSpatialAnchor.UnboundAnchor unboundAnchor){
		var pose = unboundAnchor.Pose;
		var spatialAnchor = Instantiate(pillarPrefab, NormalizePosition(pose.position), pose.rotation);
      //  unboundAnchor.BindTo(spatialAnchor);
		return spatialAnchor;
	}

	public async Task TryLoadMap(string name){
		RestartMapping();
		map = MapManager.Instance.GetMap(name);
		if (map == null)
		{
			Debug.Log("Map not found");
			return;
		}
        Debug.Log("Map found");
         if (mapLoaded)
        {
            Debug.Log("Map already loaded");
            // destroy all previous walls
            RestartMapping();
			mapLoaded = false;
        }
        ChangeStatus(State.LocatingAnchors);
        // try to locate the two anchors
        tempAnchors.Clear();
		var uuids = new Guid[2];
		uuids[0] = new Guid(map.anchor1);
		uuids[1] = new Guid(map.anchor2);

        Debug.Log("Loading anchors2");
        OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions{
			Timeout = 5,
			StorageLocation = OVRSpace.StorageLocation.Local,
			Uuids = uuids
		};

        Debug.Log("Loading anchors");
		OVRSpatialAnchor.UnboundAnchor[] anchors = null;
		try
        {
            anchors = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(options);
        } catch (Exception ex)
		{
			Debug.Log(ex);
		}
        if (anchors == null || anchors.Length != 2)
        {
            Debug.Log("Didn't find anchors");
            ChangeStatus(State.Initial);
			return;
        }
        Debug.Log("Found anchors: " + anchors.Length);
        List<OVRTask<bool>> tasks = new();
		foreach(var anchor in anchors){
			if(anchor.Localized){
				tempAnchors.Add(anchor);
			} else if(!anchor.Localized && !anchor.Localizing){
				tempAnchors.Add(anchor);
				tasks.Add(anchor.LocalizeAsync(5));
			}
		}
		// wait for all anchors to localize
		// if something fails, reset state to initial
		foreach(var task in tasks)
		{
			bool success = await task;
			if (!success)
			{
				ChangeStatus(State.Initial);
				tempAnchors.Clear();
				return;
			}
		}
		// Now both anchors are localized
		await OnAllLocated();

    }

	

	public void ChangeStatus(State state)
	{
		if (state == State.Floor)
		{
			floor.GetComponent<MeshRenderer>().enabled = true;
            floor.transform.position = new Vector3(floor.transform.position.x, hand.transform.position.y, floor.transform.position.z);
        }
		else if (state == State.Mapping)
		{
			pillar.SetActive(true);
		}
		//RectTransform d = new RectTransform();
		if(state != State.Mapping){
			Destroy(wallToLookAt);
			wallToLookAt = null;
			pillar.SetActive(false);
		}
		if(state != State.Floor){
			floor.GetComponent<MeshRenderer>().enabled = false;
		}
		_state = state;
	}
	void Start()
	{
		ChangeStatus(State.Initial);
	}

	public async void SaveCurrentMap()
	{
		Debug.Log("Trying to save map...");
		if(pillars.Count < 3)
		{
			return;
		}
		List<Vector2> points = new List<Vector2>();
		foreach(var pillar in pillars)
		{
			points.Add(new Vector2(pillar.transform.position.x, pillar.transform.position.z));
		}
        // save first pillar
        await pillars[0].SaveAsync();
        await pillars[1].SaveAsync();

		int num = 0;
        var maps = MapManager.Instance.GetMaps();
        while (maps.Select(m => m.Name).Contains(num.ToString()))
		{
			num++;
		}
        MapManager.Instance.CreateMap(num.ToString(), points, pillars[0].Uuid.ToString(), pillars[1].Uuid.ToString());
	}

	public void OnSetMarker()
    {
		if (_state != State.Mapping) return;
        OVRSpatialAnchor pillarCopy = Instantiate(pillarPrefab, pillar.transform.position, Quaternion.identity);
        _ = AnchorCreated(pillarCopy);
	}

	private async Task<bool> AnchorCreated(OVRSpatialAnchor anchor)
	{
		int i = 0;
        while (!anchor.Created && !anchor.Localized)
		{
			await Task.Delay((int)(Time.deltaTime * 1000));
            if (++i > 5/Time.deltaTime || !anchor)
            {
				if (anchor)
                {
                    Destroy(anchor.gameObject);
                }
				return false;
            }
        }
        pillars.Add(anchor);
		return true;
	}

	public void RestartMapping()
	{
		foreach (OVRSpatialAnchor anchor in pillars)
		{
			Destroy(anchor.gameObject);
		}
		foreach(GameObject w in walls){
			Destroy(w);
		}
		pillars.Clear();
		walls.Clear();
	}

	Vector3 GetPointingLoc()
	{
		Vector3 pos;
		interact.TryGetHitInfo(out pos, out _, out _, out _);
		return pos;
	}

	// update pillars y-coordinates
	void UpdatePillars()
	{
		for (int i = 0; i < pillars.Count; i++)
		{
			pillars[i].transform.position = NormalizePosition(pillars[i].transform.position);
		}

	}
	void UpdateWalls()
	{
		for (int i = 0; i < pillars.Count - 1; i++)
		{
			OVRSpatialAnchor pillar1 = pillars[i];
			OVRSpatialAnchor pillar2 = pillars[i + 1];

            Vector3 wallPosition = (pillar1.transform.position + pillar2.transform.position) / 2;
            Quaternion wallRotation = pillar1.transform.rotation;
			GameObject wall;
            if (walls.Count <= i)
			{

				// Apply transformations to the wall
				wall = Instantiate(wallPrefab);

				walls.Add(wall);
			} else
			{
				wall = walls[i];
			}
			wall.transform.position = wallPosition;
			wall.transform.rotation = wallRotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(pillar1.transform.position, pillar2.transform.position));
            wall.transform.LookAt(pillar1.transform.position);
        }
	}

	Vector3 NormalizePosition(Vector3 pos){
		return new Vector3(pos.x , pillar.transform.localScale.y / 2 + floor.transform.position.y, pos.z);
	}

	void AddPreviewWall(Vector3 pos){
		pos = NormalizePosition(pos);
		if(pillars.Count > 0){
			OVRSpatialAnchor pillar = pillars[pillars.Count - 1];
			Vector3 wallPosition = (pillar.transform.position + pos) / 2;
			Quaternion wallRotation = pillar.transform.rotation;
			if(wallToLookAt == null){
				wallToLookAt = Instantiate(wallPrefab, wallPosition, wallRotation);
			}
			wallToLookAt.transform.position = NormalizePosition(wallPosition);
			wallToLookAt.transform.localScale = new Vector3(wallToLookAt.transform.localScale.x, wallToLookAt.transform.localScale.y, Vector3.Distance(pillar.transform.position, pos));
			wallToLookAt.transform.LookAt(pillar.transform.position);
		}
	}

	void Update()
	{
		if (_state == State.Mapping)
		{
			Vector3 pos = GetPointingLoc();
			pillar.transform.position = NormalizePosition(pos);

			AddPreviewWall(pos);
		}

		if (_state == State.Floor)
		{
			floor.transform.position = new Vector3(floor.transform.position.x, Mathf.Min(floor.transform.position.y, hand.transform.position.y), floor.transform.position.z);
            UpdatePillars();
        }
		UpdateWalls();
	}

}