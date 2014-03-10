using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	Material originalMaterial;
	int overlapCount = 0;
	bool placed = false;

	int layerMask;

	
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
			if (overlapCount == 0)
				renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f) * Color.red;
			else
				renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f) * Color.green;
		}
	}

	/// <summary>
	/// Places the object.
	/// </summary>
	public bool Place()
	{
		if (overlapCount != 0)
		{
			placed = true;
			renderer.material = originalMaterial;
			collider.isTrigger = false;
			return true;
		}
		else
			return false;
	}

	void OnTriggerEnter(Collider other)
	{
		//If the item overlaps a terrain object
		if (other.gameObject.layer == layerMask)
		{
			Debug.Log("Overlap: " + overlapCount);
			overlapCount++;
		}
	}

	void OnTriggerExit(Collider other)
	{
		//If the item no longer overlaps a terrain object
		if (other.gameObject.layer == layerMask)
		{
			Debug.Log("Overlap: " + overlapCount);
			overlapCount--;
		}
	}
}
