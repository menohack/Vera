using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	/// <summary>
	/// The starting material of the item.
	/// </summary>
	Material originalMaterial;

	/// <summary>
	/// Counts the number of intersections with the environment. Zero means no intersection.
	/// </summary>
	int overlapCount = 0;

	/// <summary>
	/// Whether this item has been placed yet.
	/// </summary>
	bool placed = false;

	/// <summary>
	/// The layer of the environment.
	/// </summary>
	int layerMask;

	/// <summary>
	/// The minimum distance two buildings that are not connected may be.
	/// </summary>
	public float minBuildingDistance = 10.0f;
	
	/// <summary>
	/// Initialization.
	/// </summary>
	void Start () {
		originalMaterial = renderer.material;
		layerMask = LayerMask.NameToLayer("Environment");
	}
	
	/// <summary>
	/// Changes the color of the object.
	/// </summary>
	void Update () {
		//If the item has not been placed make it transparent and either red or green
		if (!placed)
		{
			renderer.material = new Material(originalMaterial);
			renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
			if (CanPlace())
				renderer.material.color *= Color.green;
			else
				renderer.material.color *= Color.red;
		}
	}

	/// <summary>
	/// Attempts to place the object.
	/// <returns>True if the object was placed.</returns>
	/// </summary>
	public bool Place()
	{
		if (CanPlace())
		{
			placed = true;
			renderer.material = originalMaterial;
			collider.isTrigger = false;
			return true;
		}
		else
			return false;
	}

	/// <summary>
	/// Tests whether the item can be placed.
	/// </summary>
	/// <returns>True if the item can be placed.</returns>
	bool CanPlace()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
			if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < minBuildingDistance)
				return false;

		if (overlapCount != 0)
			return true;
		else
			return false;
	}

	/// <summary>
	/// Called when the item intersects an Environment GameObject.
	/// </summary>
	/// <param name="other">The collider that is being intersected.</param>
	void OnTriggerEnter(Collider other)
	{
		//If the item overlaps a terrain object
		if (other.gameObject.layer == layerMask)
			overlapCount++;
	}

	/// <summary>
	/// Called when the item no longer intersects an Environment GameObject.
	/// </summary>
	/// <param name="other">The collider that is no longer being intersected.</param>
	void OnTriggerExit(Collider other)
	{
		//If the item no longer overlaps a terrain object
		if (other.gameObject.layer == layerMask)
			overlapCount--;
	}
}
