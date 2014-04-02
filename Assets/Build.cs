using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour {

	public Transform buildObject;
	public float rayDistance = 5.0f;
	bool hasItem = false;
	GameObject itemHeld;
	bool hasBuilding = false;

	Object[] buildings;
	int buildingIndex = 0;

	Inventory inventory;

	// Use this for initialization
	void Start () {
		//Load the building prefabs
		buildings = Resources.LoadAll("Buildings");

		//Load the building instances
		LoadItem("Rock");
		LoadItem("Wall");
		//LoadItem("Floor");

		inventory = gameObject.GetComponent<Inventory>();
	}

	void OnApplicationQuit()
	{
		StoreAllItems();
	}

	void LoadItem(string name)
	{
		//Debug.Log("Looking for " + name + "...");
		int i = 0;
		while (PlayerPrefs.HasKey(name + i))
		{
			string s = PlayerPrefs.GetString(name + i++);
			//Debug.Log("Found " + s);
			string[] floats = s.Split(' ');

			Vector3 position = new Vector3(float.Parse(floats[0]), float.Parse(floats[1]), float.Parse(floats[2]));
			Quaternion rotation = new Quaternion(float.Parse(floats[3]), float.Parse(floats[4]), float.Parse(floats[5]), float.Parse(floats[6]));

			Object resource = null;
			foreach (Object o in buildings)
			{
				GameObject go = o as GameObject;
				if (go.name == name)
					resource = o;
			}

			if (resource != null)
			{
				//Debug.Log("Instantiating " + name);
				GameObject building = Instantiate(resource) as GameObject;
				building.transform.position = position;
				building.transform.rotation = rotation;
				building.name = name;

				Building b = building.GetComponent<Building>();
				if (b)
					b.Place();
			}
		}
	}

	int ScrollBuildings(int delta)
	{
		int index = buildingIndex + delta;
		if (index >= buildings.Length)
			index = index % buildings.Length;
		else if (index < 0)
			index = buildings.Length - (-index) % buildings.Length;
		return index;
	}

	void EquipBuilding()
	{
		GameObject wall = Instantiate(buildings[buildingIndex]) as GameObject;
		Item i = wall.GetComponent<Item>();
		if (i != null)
			i.SetFloatPoint(transform, new Vector3(1.75f, -1.05f, 2.0f), Quaternion.Euler(-90.0f, 0, 0));
		wall.name = buildings[buildingIndex].name;
		if (wall.rigidbody)
			wall.rigidbody.isKinematic = true;

		hasItem = true;
		hasBuilding = true;
		itemHeld = wall;
	}

	/// <summary>
	/// Saves an item to PlayerPrefs so that it is loaded the next time the game is started.
	/// </summary>
	/// <param name="item"></param>
	void StoreItem(GameObject item, string key)
	{
		Vector3 p = item.transform.position;
		Quaternion r = item.transform.rotation;
		string itemString = string.Format("{0} {1} {2} {3} {4} {5} {6}", p.x, p.y, p.z, r.x, r.y, r.z, r.w);
		//Debug.Log(key + ":" + itemString);
		PlayerPrefs.SetString(key, itemString);
	}

	void StoreObjectsWithTag(string tag)
	{
		GameObject[] o = GameObject.FindGameObjectsWithTag(tag);

		Dictionary<string, int> indices = new Dictionary<string, int>();

		for (int i = 0; i < o.Length; i++)
		{
			int index = 0;
			if (indices.ContainsKey(o[i].name))
				index = indices[o[i].name];
			
			indices[o[i].name] = index+1;

			StoreItem(o[i], o[i].name + index);
		}
	}

	void StoreAllItems()
	{
		StoreObjectsWithTag("Ore");
		StoreObjectsWithTag("Building");
	}

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
			//Save the item to file
			//StoreItem(itemHeld);
			StoreAllItems();

			hasItem = false;
			hasBuilding = false;
			itemHeld = null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
			Networking.Instance.GetMap();

		if (hasItem)
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
				hasItem = false;
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
				if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask))
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
						/*
						GameObject item = hit.transform.gameObject;

						item.transform.parent = transform;
						item.transform.localPosition = new Vector3(0f, 1.0f, 3.0f);
						if (item.rigidbody)
							item.rigidbody.isKinematic = true;

						hasItem = true;
						itemHeld = item;
						*/
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
				GameObject[] gos = GameObject.FindGameObjectsWithTag("Ore");
				foreach (GameObject go in gos)
					Destroy(go);

				gos = GameObject.FindGameObjectsWithTag("Building");
				foreach (GameObject go in gos)
					Destroy(go);

				PlayerPrefs.DeleteAll();
			}
		}
		
	}
}
