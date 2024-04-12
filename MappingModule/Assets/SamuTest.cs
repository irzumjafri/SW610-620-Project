using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;


enum State
{
	Initial,
	Floor, // At this state, user sets floor level
	Mapping, // At this state, walls can be added
	Finished
}
public class SamuTest : MonoBehaviour
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
	private List<OVRSpatialAnchor> pillars = new List<OVRSpatialAnchor>();

	public OVRSpatialAnchor pillarPrefab;
	public GameObject pillar;
	private GameObject wallToLookAt;
	public const string NumUuidsPlayerPref ="numUuids";
	
	Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;

	void Awake(){
		_onLoadAnchor = OnLocalized;
		LoadAnchors();
	}

	void LoadAnchors(){
		if(!PlayerPrefs.HasKey(NumUuidsPlayerPref)){
			PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
		}
		int numUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
		if (numUuids == 0){
			return;
		}
		var uuids = new Guid[numUuids];
		for(int i = 0; i < numUuids; i++){
			var key = "uuid" + i;
			var uuid = PlayerPrefs.GetString(key);
			uuids[i] = new Guid(uuid);
		}
		OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions{
			Timeout = 0,
			StorageLocation = OVRSpace.StorageLocation.Local,
			Uuids = uuids
		};

		OVRSpatialAnchor.LoadUnboundAnchors(options, anchors => {
			if(anchors == null){
				return;
			}
			foreach(var anchor in anchors){
				if(anchor.Localized){
					_onLoadAnchor(anchor, true);
				} else if(!anchor.Localizing){
					anchor.Localize(_onLoadAnchor);
				}

			}
		});
	}

	private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success){
		if(!success) {
			return;
		}
		var pose = unboundAnchor.Pose;
		var spatialAnchor = Instantiate(pillarPrefab, pose.position, pose.rotation);
		unboundAnchor.BindTo(spatialAnchor);
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
		StartCoroutine(AnchorCreated(pillarCopy));
	}

	private IEnumerator AnchorCreated(OVRSpatialAnchor anchor)
	{
		while (!anchor.Created && !anchor.Localized)
		{
			yield return new WaitForEndOfFrame();
		}
		pillars.Add(anchor);

		anchor.Save((anchor, success) => {
			// hope for the best
		});

		if(!PlayerPrefs.HasKey(NumUuidsPlayerPref)){
			PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
		}
		int numUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
		PlayerPrefs.SetString("uuid" + numUuids, anchor.Uuid.ToString());
		PlayerPrefs.SetInt(NumUuidsPlayerPref, ++numUuids);
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