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

	// Use this for initialization
	void Start () {
		//Load the building prefabs
		buildings = Resources.LoadAll("Buildings");

		//Load the building instances
		LoadItem("Rock(Clone)");
		LoadItem("Wall(Clone)");
		LoadItem("Floor(Clone)");
	}

	void LoadItem(string name)
	{
		Debug.Log("Looking for " + name + "...");
		int i = 0;
		while (PlayerPrefs.HasKey(name + i))
		{
			string s = PlayerPrefs.GetString(name + i++);
			Debug.Log("Found " + s);
			string[] floats = s.Split(' ');

			Vector3 position = new Vector3(float.Parse(floats[0]), float.Parse(floats[1]), float.Parse(floats[2]));
			Quaternion rotation = new Quaternion(float.Parse(floats[3]), float.Parse(floats[4]), float.Parse(floats[5]), float.Parse(floats[6]));

			Object resource = null;
			foreach (Object o in buildings)
			{
				GameObject go = o as GameObject;
				if (go.name + "(Clone)" == name)
					resource = o;
			}

			if (resource != null)
			{
				Debug.Log("Instantiating " + name);
				GameObject building = Instantiate(resource) as GameObject;
				building.transform.position = position;
				building.transform.rotation = rotation;
			}
		}
	}

	int ScrollBuildings(int delta)
	{
		Debug.Log("Delta " + delta);
		int index = buildingIndex + delta;
		if (index >= buildings.Length)
			index = index % buildings.Length;
		else if (index < 0)
			index = buildings.Length - (-index) % buildings.Length;
		Debug.Log("Output " + index);
		return index;
	}

	void EquipBuilding()
	{
		GameObject wall = Instantiate(buildings[buildingIndex]) as GameObject;
		wall.transform.parent = transform;
		wall.transform.localPosition = new Vector3(0f, 2.0f, 3.0f);
		wall.transform.localRotation = Quaternion.Euler(0, 90, 0);
		wall.collider.enabled = false;
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
	void StoreItem(GameObject item, int index)
	{
		Vector3 p = item.transform.position;
		Quaternion r = item.transform.rotation;
		string itemString = string.Format("{0} {1} {2} {3} {4} {5} {6}", p.x, p.y, p.z, r.x, r.y, r.z, r.w);
		string key = item.name + "" + index;
		Debug.Log(key + ":" + itemString);
		PlayerPrefs.SetString(key, itemString);
	}

	void StoreAllItems()
	{
		GameObject[] o = GameObject.FindGameObjectsWithTag("Ore");
		GameObject[] b = GameObject.FindGameObjectsWithTag("Building");

		for (int i = 0; i < o.Length; i++)
			StoreItem(o[i], i);

		for (int i = 0; i < b.Length; i++)
			StoreItem(b[i], i);
	}

	void PlaceItem()
	{
		itemHeld.transform.parent = null;
		if (itemHeld.rigidbody)
			itemHeld.rigidbody.isKinematic = false;
		itemHeld.collider.enabled = true;

		//Save the item to file
		//StoreItem(itemHeld);
		StoreAllItems();

		hasItem = false;
		hasBuilding = false;
		itemHeld = null;
	}

	// Update is called once per frame
	void Update () {
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

				if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask) && (hit.transform.tag == "Ore" || hit.transform.tag == "Building"))
				{
					GameObject item = hit.transform.gameObject;

					Debug.Log("Hit " + item.name);

					item.transform.parent = transform;
					item.transform.localPosition = new Vector3(0f, 1.0f, 3.0f);
					if (item.rigidbody)
						item.rigidbody.isKinematic = true;

					hasItem = true;
					itemHeld = item;
				}
				else
					Debug.Log("Miss");
			}
			else if (buildings.Length > 0 && Input.GetButtonDown("Fire2"))
			{
				EquipBuilding();
			}
			else if (Input.GetAxis("Build") == 1.0f)
			{
				GameObject go = Instantiate(buildObject, transform.position + transform.forward * 5.0f, transform.rotation) as GameObject;
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				PlayerPrefs.DeleteAll();
			}
		}
		
	}
}
