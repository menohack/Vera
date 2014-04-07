﻿using UnityEngine;
using System.Collections;

public abstract class Building : Item {

	/// <summary>
	/// The hit points of the building.
	/// </summary>
	private float health = 100f;

	/// <summary>
	/// Counts the number of intersections with the environment. Zero means no intersection.
	/// </summary>
	protected int overlapCount = 0;

	/// <summary>
	/// The layer of the environment.
	/// </summary>
	int layerMask;

	/// <summary>
	/// The minimum distance two buildings that are not connected may be.
	/// </summary>
	public float minBuildingDistance = 2.0f;

	/// <summary>
	/// The original material of the wall.
	/// </summary>
	Material originalMaterial;

	/// <summary>
	/// The colors used for shading an unbuild building.
	/// </summary>
	Color red, green;

	/// <summary>
	/// Initialization.
	/// </summary>
	void Start () {
		originalMaterial = renderer.material;
		layerMask = LayerMask.NameToLayer("Environment");

		renderer.material = new Material(renderer.material);
		red = renderer.material.color;
		red *= new Color(0.9f, 0.9f, 0.9f);
		red += 0.9f * Color.red;
		red.a = 0.8f;

		green = renderer.material.color;
		green *= new Color(0.9f, 0.9f, 0.9f);
		green += 0.9f * Color.green;
		green.a = 0.8f;
		
	}

	public override void SetGhostPosition(Transform heldPosition)
	{
		return;
	}

	/// <summary>
	/// Damages the building for dmg damage.
	/// </summary>
	/// <param name="dmg">The positive number of damage points to inflict.</param>
	public void Damage(float dmg)
	{
		health -= dmg;
	}

	/// <summary>
	/// Checks if the building is alive. If it is not it destroys it.
	/// </summary>
	/// <returns>True if the building is alive.</returns>
	public bool IsAlive()
	{
		if (health <= 0)
		{
			Destroy(gameObject);
			return false;
		}
		else
			return true;
	}

	/// <summary>
	/// Changes the color of the object & Destroys the object when health is less than or equal to zero.
	/// </summary>
	protected override void Update ()
	{
		//If the item has not been placed make it transparent and either red or green
		if (!placed)
		{
			if (CanPlace())
				renderer.material.color = green;
			else
				renderer.material.color = red;
		}
	}

	/// <summary>
	/// Attempts to place the object.
	/// <returns>True if the object was placed.</returns>
	/// </summary>
	public override bool Place()
	{
		if (CanPlace())
		{
			placed = true;
			renderer.material = originalMaterial;
			transform.parent = null;
			
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
	protected override bool CanPlace()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
			if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < minBuildingDistance)
				return false;

		return true;
		return IntersectingTerrain();
	}

	protected bool IntersectingTerrain()
	{
		return overlapCount != 0;
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
		Debug.Log(overlapCount);
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
		Debug.Log(overlapCount);
	}
}
