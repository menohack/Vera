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
		buildings = Resources.LoadAll("Buildings");
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

		hasItem = true;
		hasBuilding = true;
		itemHeld = wall;
	}

	// Update is called once per frame
	void Update () {
		if (hasItem)
		{
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (Input.GetButtonDown("Fire1"))
			{
				//Drop the item
				itemHeld.transform.parent = null;
				if (itemHeld.rigidbody)
					itemHeld.rigidbody.isKinematic = false;
				hasItem = false;
				hasBuilding = false;
				itemHeld = null;
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
				int layerMask = 1 << LayerMask.NameToLayer("Ore") | 1 << LayerMask.NameToLayer("Building");

				if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask))
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
