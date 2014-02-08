using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

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
		StreamReader reader = new StreamReader("Buildings.xml");
		XDocument doc = XDocument.Load("Buildings.xml");

		Debug.Log(doc.FirstNode);
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

		hasItem = true;
		hasBuilding = true;
		itemHeld = wall;
	}

	/// <summary>
	/// Saves an item to file.
	/// </summary>
	/// <param name="item"></param>
	void StoreItem(GameObject item)
	{
		FileStream fs = File.OpenWrite("Buildings.xml");
		StreamWriter writer = new StreamWriter(fs);
		writer.Write(new XElement("Buildings", new XElement("Building", item.ToString())));
		writer.Close();
	}

	void PlaceItem()
	{
		itemHeld.transform.parent = null;
		if (itemHeld.rigidbody)
			itemHeld.rigidbody.isKinematic = false;
		itemHeld.collider.enabled = true;

		//Save the item to file
		StoreItem(itemHeld);

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
		}
		
	}
}
