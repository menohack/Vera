using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Build : MonoBehaviour {

	//BC: Note: the changes introduced with this are quick fixes, and this script *really* needs to be refactored from the ground up
	enum BuildingState {original, Rot1};
	
	//current 'state' of the building held
	BuildingState currentState;

	/// The distance of a ray in front of the character for mining and picking up objects.
	public static float RAYCAST_DISTANCE = 5.0f;
	public bool issueGUOs = true; /** Issue a graph update object after placement */
	public bool direct = false; /** Flush Graph Updates directly after placing. Slower, but updates are applied immidiately */
	// allows you to press R to destroy all buildings, see debug statements
	public bool DEBUG = false;
	/// The currently held item, which can be a building as well.
	GameObject itemHeld = null;
	/// If the player is holding ("ghosting") a potential building.
	public bool hasBuilding = false;
	/// The list of building prefabs available to the player.
	Object[] buildings;
	/// The index of the currently selected building from buildings.
	int buildingIndex = 0;

	/// The Player's inventory.
	Inventory inventory;

	Transform holdPoint;

	NetworkController nc;

	/// Load the building prefabs (used for instantiating them) and the inventory.
	void Start () {
		//Load the building prefabs
		buildings = Resources.LoadAll("Buildings");

		inventory = gameObject.GetComponent<Inventory>();

		GameObject hp = GameObject.Find("HoldPoint");
		holdPoint = hp.transform;

		nc = FindObjectOfType<NetworkController>();

	}

	/// <summary>
	/// Spawn a "ghost" building in front of the player for placement.
	/// </summary>
	void EquipBuilding()
	{
		GameObject buildingPrefab = buildings[buildingIndex] as GameObject;
		GameObject currentBuilding = Utility.InstantiateHelper(buildingPrefab, buildingPrefab.transform.position, buildingPrefab.transform.rotation);
			
		Item i = currentBuilding.GetComponent<Item>();
		if (i != null)
			i.SetGhostPosition(holdPoint);
		currentBuilding.name = buildings[buildingIndex].name;
		if (currentBuilding.rigidbody)
			currentBuilding.rigidbody.isKinematic = true;

		hasBuilding = true;
		itemHeld = currentBuilding;
		currentState = BuildingState.original;
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

			hasBuilding = false;
			itemHeld = null;
		}
	}

	/// <summary>
	/// The logic for spawning objects and attacking. Part of this should be extracted to more appropriate classes.
	/// </summary>
	void Update () {
		//BC: I'm personally unsure what this is for
		if (Time.timeScale == 0f)
			return;
		//there is a currently held item
		if (itemHeld != null) {
			//Drop the item
			if (Input.GetButtonDown ("Fire1")) 
			{
				PlaceItem ();
			// unequips building
			} else if (hasBuilding) {
				int tempBuildingIndex = getIndexByKey ();
				if (tempBuildingIndex >= 0 && tempBuildingIndex < buildings.Length) {
					//this is where we handle the 'rotating' functionality
					if (buildingIndex == tempBuildingIndex && currentState == BuildingState.original) {
						Debug.Log ("this is when we would rotate"); //just a placeholder for now
						//move to next state
						currentState = BuildingState.Rot1;
						itemHeld.transform.Rotate(Vector3.up, 90, Space.World);					
						//this is where we remove the building
					} else if (buildingIndex == tempBuildingIndex && currentState == BuildingState.Rot1) {
						Utility.DestroyHelper(itemHeld);
						itemHeld = null;
						hasBuilding = false;
					// if a new number is selected, switch to that object
					} else {
						buildingIndex = tempBuildingIndex;
						Utility.DestroyHelper(itemHeld);
						itemHeld = null;
						EquipBuilding ();
					}
				}
			}
		} else {
			//equip a building
			if (buildings.Length > 0) {
				int tempBuildingIndex = getIndexByKey ();
				if (tempBuildingIndex >= 0 && tempBuildingIndex < buildings.Length) {
					buildingIndex = tempBuildingIndex;
					EquipBuilding ();
				}
			}
		}	
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

	public GameObject GetItemHeld(){
		return itemHeld;
	}
}
