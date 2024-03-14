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


enum State
{
	Initial,
	LocatingAnchors,
	Floor, // At this state, user sets floor level
	Mapping, // At this state, walls can be added
	Finished
}
public class Mapper : MonoBehaviour
{
	public XRRayInteractor interact; // the rayinteractor used for raycasting in mapping
	public GameObject wallPrefab;
	public Button startButton;
	public Button resetButton;
	public TextMeshProUGUI startButtonText;
	public GameObject hand;
	public GameObject floor;
	private List<GameObject> walls = new List<GameObject>();
	private State _state = State.Initial;
	private bool mapLoaded = false;
	private Map map;
	private List<OVRSpatialAnchor> pillars = new List<OVRSpatialAnchor>();
	private List<OVRSpatialAnchor.UnboundAnchor> tempAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();

	public OVRSpatialAnchor pillarPrefab;
	public GameObject pillar;
	private GameObject wallToLookAt;
	public const string NumUuidsPlayerPref ="numUuids";
	

	void Awake(){
		//LoadAnchors();
	}

	void OnAllLocated(){
		if(tempAnchors.Count != 2) {
			return;
		}
		mapLoaded = true;
		

		foreach(var unboundAnchor in tempAnchors){
			pillars.Add(BindUnbound(unboundAnchor));
		}
		if(tempAnchors[0].Uuid.ToString() != map.anchor1){
			pillars.Reverse();
		}
		// we can delete temp anchors now
		tempAnchors.Clear();

		// now we need to draw all the other pillars too
		var points = MapManager.Instance.GetUnityCoordinates(map, V3ToV2(pillars[0].transform.position), V3ToV2(pillars[1].transform.position));

		for(int i = 2; i < points.Count; i++){
			OVRSpatialAnchor pillarCopy = Instantiate(pillarPrefab, new Vector3(points[i].x, floor.transform.position.y, points[i].y), Quaternion.identity);

			AnchorCreated(pillarCopy);
		}
		

	}

	Vector2 V3ToV2(Vector3 vector){
		return new Vector2(vector.x, vector.z);
	}

	OVRSpatialAnchor BindUnbound(OVRSpatialAnchor.UnboundAnchor unboundAnchor){
		var pose = unboundAnchor.Pose;
		var spatialAnchor = Instantiate(pillarPrefab, pose.position, pose.rotation);
		unboundAnchor.BindTo(spatialAnchor);
		return spatialAnchor;
	}

    async void TryLoadMap(string name){
		map = MapManager.Instance.GetMap(name);
		if(map == null){
			return;
		}
		if(mapLoaded){
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

		OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions{
			Timeout = 5,
			StorageLocation = OVRSpace.StorageLocation.Local,
			Uuids = uuids
		};

		var anchors = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(options);
		if(anchors == null || anchors.Length != 2){
			ChangeStatus(State.Initial);
			return;
		}
		List<OVRTask<bool>> tasks = new();
		foreach(var anchor in anchors){
			if(anchor.Localized){
				tempAnchors.Add(anchor);
			} else if(!anchor.Localizing){
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
		OnAllLocated();

    }

	

	void ChangeStatus(State state)
	{
		if (state == State.Initial)
		{
			startButtonText.text = "Start flooring";
		}
		else if (state == State.Floor)
		{
			startButtonText.text = "End flooring";
			floor.GetComponent<MeshRenderer>().enabled = true;
		}
		else if (state == State.Mapping)
		{
			startButtonText.text = "end walling";
			pillar.SetActive(true);
		}
		else if (state == State.Finished)
		{
			startButtonText.text = "walling finished";
		} else if (state == State.LocatingAnchors){
			startButtonText.text = "Locating anchors";
		}
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
		startButton.onClick.AddListener(OnStartButtonClick);
		resetButton.onClick.AddListener(RestartMapping);

		ChangeStatus(State.Initial);

	}

	void OnStartButtonClick()
	{
		if (_state == State.Initial)
		{
			ChangeStatus(State.Floor);
		}
		else if (_state == State.Floor)
		{
			ChangeStatus(State.Mapping);
		}
		else if (_state == State.Mapping)
		{
			ChangeStatus(State.Finished);
		}
	}

	public void OnSetFloorLevel()
	{
		ChangeStatus(State.Mapping);
	}

	public void OnSetMarker()
	{
		if (_state == State.Floor)
		{
			OnSetFloorLevel();
			return;
		}
		OVRSpatialAnchor pillarCopy = Instantiate(pillarPrefab, pillar.transform.position, Quaternion.identity);
		AnchorCreated(pillarCopy);
	}

	private async void AnchorCreated(OVRSpatialAnchor anchor)
	{
		while (!anchor.Created && !anchor.Localized)
		{
			await Task.Delay((int)Time.deltaTime * 1000);
		}
		pillars.Add(anchor);

		bool success = await anchor.SaveAsync();

		if (!success)
		{
			// should probably do something
		}
	}

	void RestartMapping()
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
		ChangeStatus(State.Initial);
	}

	Vector3 GetPointingLoc()
	{
		Vector3 pos;
		interact.TryGetHitInfo(out pos, out _, out _, out _);
		return pos;
	}

	void UpdateWalls()
	{
		for (int i = 0; i < pillars.Count - 1; i++)
		{
			OVRSpatialAnchor pillar1 = pillars[i];
			OVRSpatialAnchor pillar2 = pillars[i + 1];

			if (walls.Count <= i)
			{
				Vector3 wallPosition = (pillar1.transform.position + pillar2.transform.position) / 2;
				Quaternion wallRotation = pillar1.transform.rotation;

				// Apply transformations to the wall
				GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
				wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(pillar1.transform.position, pillar2.transform.position));
				wall.transform.LookAt(pillar1.transform.position);

				walls.Add(wall);
			}
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
			floor.transform.position = hand.transform.position;
		}
		UpdateWalls();
	}

}