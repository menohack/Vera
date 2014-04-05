using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour {

	/// <summary>
	/// The distance of a ray in front of the character for mining and picking up objects.
	/// </summary>
	public static float RAYCAST_DISTANCE = 5.0f;

	/// <summary>
	/// The currently held item, which can be a building as well.
	/// </summary>
	GameObject itemHeld = null;

	/// <summary>
	/// If the player is holding ("ghosting") a potential building.
	/// </summary>
	bool hasBuilding = false;

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

	/// <summary>
	/// Load the building prefabs (used for instantiating them) and the inventory.
	/// </summary>
	void Start () {
		//Load the building prefabs
		buildings = Resources.LoadAll("Buildings");

		inventory = gameObject.GetComponent<Inventory>();
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
		GameObject wall = Instantiate(buildings[buildingIndex]) as GameObject;
		Item i = wall.GetComponent<Item>();
		if (i != null)
			i.SetFloatPoint(transform, new Vector3(0f, -1.05f, 2.0f), Quaternion.Euler(-90.0f, 0, 0));
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
		Wall wallScript = itemHeld.GetComponent<Wall>();
		if (wallScript)
		{
			if (inventory && inventory.GetOre() >= Wall.WALL_COST_ORE && inventory.GetWood() >= Wall.WALL_COST_WOOD)
			{
				inventory.RemoveOre(Wall.WALL_COST_ORE);
				inventory.RemoveWood(Wall.WALL_COST_WOOD);
			}
			else
				return;
		}

		Item itemScript = itemHeld.GetComponent<Item>();
		
		if (itemScript && itemScript.Place())
		{
			if (itemHeld.rigidbody)
				itemHeld.rigidbody.isKinematic = false;
			itemHeld.collider.enabled = true;

			
			if (wallScript)
			{
				wallScript.SetWall();
			}

			hasBuilding = false;
			itemHeld = null;
		}
	}

	/// <summary>
	/// The logic for spawning objects and attacking. Part of this should be extracted to more appropriate classes.
	/// </summary>
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
			Networking.Instance.GetMap();

		if (itemHeld != null)
		{
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (Input.GetButtonDown("Fire1"))
			{
				//Drop the item
				PlaceItem();
			}
			else if (hasBuilding && Input.GetButtonDown("Fire2"))
			{
				Destroy(itemHeld);
				itemHeld = null;
				hasBuilding = false;
			}
			else if (hasBuilding && scroll != 0.0f)
			{
				int scrollIndices = (int)(scroll * 10.0f);
				int tempBuildingIndex = ScrollBuildings(scrollIndices);
				if (buildingIndex != tempBuildingIndex)
				{
					buildingIndex = tempBuildingIndex;
					Destroy(itemHeld);
					itemHeld = null;
					EquipBuilding();
				}
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire1"))
			{
				RaycastHit hit;
				int layerMask = 1 << LayerMask.NameToLayer("Environment");


				//TODO: destructible (buildings) and attackable (NPC/other player) tags?
				if (Physics.Raycast(transform.position, transform.forward, out hit, RAYCAST_DISTANCE, layerMask))
				{
					if (hit.transform.tag == "Building")
					{
						GameObject item = hit.transform.gameObject;

						Building hp = item.GetComponent<Building>();
						if (hp != null)
							hp.Damage(25);
							hp.IsAlive();
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
								Debug.Log("Error!!!!!!!11eleven!");
						}
					}
				}
			}
			else if (buildings.Length > 0 && Input.GetButtonDown("Fire2"))
			{
				EquipBuilding();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				GameObject[] gos = GameObject.FindGameObjectsWithTag("Building");
				foreach (GameObject go in gos)
					Destroy(go);

				PlayerPrefs.DeleteAll();
			}
		}
		
	}
}
