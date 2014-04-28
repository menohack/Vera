using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Build : MonoBehaviour {

	/// <summary>
	/// The distance of a ray in front of the character for mining and picking up objects.
	/// </summary>
	public static float RAYCAST_DISTANCE = 5.0f;

	public bool issueGUOs = true; /** Issue a graph update object after placement */
	public bool direct = false; /** Flush Graph Updates directly after placing. Slower, but updates are applied immidiately */

	// allows you to press R to destroy all buildings, see debug statements
	public bool DEBUG = false;

	/// <summary>
	/// The currently held item, which can be a building as well.
	/// </summary>
	GameObject itemHeld = null;

	/// <summary>
	/// If the player is holding ("ghosting") a potential building.
	/// </summary>
	public bool hasBuilding = false;

	/// <summary>
	/// The list of building prefabs available to the player.
	/// </summary>
	Object[] buildings;

	/// <summary>
	/// The index of the currently selected building from buildings.
	/// </summary>
	int buildingIndex = 0;

	/// <summary>
	/// The Player's inventory.
	/// </summary>
	Inventory inventory;

	Transform holdPoint;

	public GameObject arrowPrefab;

	NetworkController nc;

	/// <summary>
	/// Load the building prefabs (used for instantiating them) and the inventory.
	/// </summary>
	void Start () {
		//Load the building prefabs
		buildings = Resources.LoadAll("Buildings");

		inventory = gameObject.GetComponent<Inventory>();

		GameObject hp = GameObject.Find("HoldPoint");
		holdPoint = hp.transform;

		nc = FindObjectOfType<NetworkController>();
	}


	/// <summary>
	/// Scrolls through the array of buildings by delta positions. Delta may be negative.
	/// </summary>
	/// <param name="delta">A positive or negative value indicating the number of array positions to move.</param>
	/// <returns>The new index of the array.</returns>
	int ScrollBuildings(int delta)
	{
		int index;
		if (buildingIndex + delta == 0)
			return 0;
		else if (buildingIndex + delta < 0)
			index = buildings.Length - ((buildingIndex + delta) * -1) % buildings.Length -1;
		else
			index = (buildingIndex + delta) % buildings.Length;
		return index;
	}

	/// <summary>
	/// Spawn a "ghost" building in front of the player for placement.
	/// </summary>
	void EquipBuilding()
	{
		GameObject wall;
		if (nc != null && nc.Connected())
			wall = Network.Instantiate(buildings[buildingIndex], holdPoint.position, holdPoint.rotation, 0) as GameObject;
		else
			wall = Instantiate(buildings[buildingIndex]) as GameObject;
			
		Item i = wall.GetComponent<Item>();
		if (i != null)
			i.SetGhostPosition(holdPoint);
		wall.name = buildings[buildingIndex].name;
		if (wall.rigidbody)
			wall.rigidbody.isKinematic = true;

		hasBuilding = true;
		itemHeld = wall;
	}

	/// <summary>
	/// Places the currently-held item in its current position.
	/// </summary>
	void PlaceItem()
	{
		bool spend = false;
		Building buildingScript = itemHeld.GetComponent<Building>();
		if (buildingScript)
		{
			if (inventory && inventory.GetOre() >= buildingScript.GetOreCost() && inventory.GetWood() >= buildingScript.GetWoodCost())
				spend = true;
			else
				return;
		}

		//update gridGraph to account for new obstacle
		if (issueGUOs) {
			Bounds b = itemHeld.collider.bounds;
			//Pathfinding.Console.Write ("// Placing Object\n");
			GraphUpdateObject guo = new GraphUpdateObject(b);
			AstarPath.active.UpdateGraphs (guo);
			if (direct) {
				//Pathfinding.Console.Write ("// Flushing\n");
				AstarPath.active.FlushGraphUpdates();
			}
		}

		Item itemScript = itemHeld.GetComponent<Item>();
		
		if (itemScript && itemScript.Place())
		{
			if (spend)
			{
				inventory.RemoveOre(buildingScript.GetOreCost());
				inventory.RemoveWood(buildingScript.GetWoodCost());
			}
			if (itemHeld.rigidbody)
				itemHeld.rigidbody.isKinematic = false;
			itemHeld.collider.enabled = true;


			hasBuilding = false;
			itemHeld = null;
		}
	}

	/// <summary>
	/// The logic for spawning objects and attacking. Part of this should be extracted to more appropriate classes.
	/// </summary>
	void Update () {
		if (Time.timeScale == 0f)
			return;
		
		if (itemHeld != null) {
			float scroll = Input.GetAxis ("Mouse ScrollWheel");
			if (Input.GetButtonDown ("Fire1")) {
				//Drop the item
				PlaceItem ();
			} else if (hasBuilding && Input.GetButtonDown ("Fire2")) {
				DestroyHelper(itemHeld);
				itemHeld = null;
				hasBuilding = false;
			} else if (hasBuilding && scroll != 0.0f) {
				int scrollIndices = (int)(scroll * 10.0f);
				int tempBuildingIndex = ScrollBuildings (scrollIndices);
				if (buildingIndex != tempBuildingIndex) {
					buildingIndex = tempBuildingIndex;
					DestroyHelper(itemHeld);
					itemHeld = null;
					EquipBuilding ();
				}
				//by key
			} else if (hasBuilding && scroll == 0.0f) {
				int tempBuildingIndex = getIndexByKey ();
				if (tempBuildingIndex >= 0 && tempBuildingIndex < buildings.Length) {
					if (buildingIndex == tempBuildingIndex) {
						DestroyHelper(itemHeld);
						itemHeld = null;
						hasBuilding = false;
					} else {
						buildingIndex = tempBuildingIndex;
						DestroyHelper(itemHeld);
						itemHeld = null;
						EquipBuilding ();
					}
				}
			}
		} else {
			//this is the resource gathering code
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit;
				int layerMask = 1 << LayerMask.NameToLayer ("Environment");

				if (Physics.Raycast(transform.position + (Vector3.up * 1.5f), transform.forward, out hit, RAYCAST_DISTANCE, layerMask))
				{
					if (hit.transform.tag == "Building")
					{
						GameObject item = hit.transform.gameObject;
						Health hp = item.GetComponent<Health>();
						if (hp != null)
							hp.Damage(25);
					}
					else if (hit.transform.tag == "Ore" || hit.transform.tag == "Tree")
					{
						Resource resource = hit.transform.gameObject.GetComponent<Resource>();
						int gatherCount = resource.Gather(1);
						if (gatherCount > 0)
						{
							if (resource is Tree)
								inventory.AddWood(gatherCount);
							else if (resource is Ore)
								inventory.AddOre(gatherCount);
							else
								Debug.LogError("No such resource");
						}
					}
				}
			}
			else if (buildings.Length > 0 && Input.GetButtonDown("Fire2"))
				EquipBuilding();
			else if (Input.GetKeyDown(KeyCode.F))
			{
				GameObject arrow;
				if (nc != null && nc.Connected())
					arrow = Network.Instantiate(arrowPrefab, holdPoint.position, transform.rotation, 0) as GameObject;
				else
				{
					arrow = Instantiate(arrowPrefab) as GameObject;
					arrow.transform.position = holdPoint.position;
					arrow.transform.rotation = transform.rotation;
				}
				Arrow arrowScript = arrow.GetComponent<Arrow>();
				if (arrowScript)
					arrowScript.Shoot(gameObject.transform.forward);
			}
			else if (buildings.Length > 0) {
				int tempBuildingIndex = getIndexByKey ();
				if (tempBuildingIndex >= 0 && tempBuildingIndex < buildings.Length) {
					buildingIndex = tempBuildingIndex;
					EquipBuilding ();
				}
			}
			else if (DEBUG && Input.GetKeyDown(KeyCode.R))
			{
				GameObject[] gos = GameObject.FindGameObjectsWithTag("Building");
				foreach (GameObject go in gos)
					if (nc != null && nc.Connected())
					DestroyHelper(go);
			}
		}	
	}

	void DestroyHelper(GameObject gameObject)
	{
		if (nc != null && nc.Connected())
			Network.Destroy(gameObject);
		else
			Destroy(gameObject);
	}


	private int getIndexByKey() {
		int alphaNumDown = -1; //stands for none

		if (Input.GetKeyDown (KeyCode.Alpha1))
			alphaNumDown = 0;
		else if (Input.GetKeyDown (KeyCode.Alpha2)) 
			alphaNumDown = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha3)) 
			alphaNumDown = 2;
		else if (Input.GetKeyDown (KeyCode.Alpha4)) 
			alphaNumDown = 3;
		else if (Input.GetKeyDown (KeyCode.Alpha5)) 
			alphaNumDown = 4;
		else if (Input.GetKeyDown (KeyCode.Alpha6)) 
			alphaNumDown = 5;
		else if (Input.GetKeyDown (KeyCode.Alpha7)) 
			alphaNumDown = 6;
		else if (Input.GetKeyDown (KeyCode.Alpha8)) 
			alphaNumDown = 7;
		else if (Input.GetKeyDown (KeyCode.Alpha9)) 
			alphaNumDown = 8;
		else if (Input.GetKeyDown (KeyCode.Alpha0)) 
			alphaNumDown = 9;

		return 
			alphaNumDown;
	}
}
