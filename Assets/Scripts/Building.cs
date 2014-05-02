using UnityEngine;
using System.Collections;
using System;

public abstract class Building : Item {

	/// <summary>
	/// The last time that OnTriggerStay was called.
	/// </summary>
	DateTime? lastTerrainTriggerStay = null;

	/// <summary>
	/// The last time that OnTriggerStay was called.
	/// </summary>
	DateTime? lastBuildingTriggerStay = null;

	/// <summary>
	/// The layer of the terrain.
	/// </summary>
	int terrainLayerMask;

	/// <summary>
	/// The layer of the buildings.
	/// </summary>
	int buildingLayerMask;

	/// <summary>
	/// The minimum distance two buildings that are not connected may be.
	/// </summary>
	public float minBuildingDistance = 1.0f;

	/// <summary>
	/// The original material of the wall.
	/// </summary>
	Material originalMaterial;

	/// <summary>
	/// The colors used for shading an unbuild building.
	/// </summary>
	Color red, green;

	public float SCALE = 4f;

	/// <summary>
	/// The transform about which the building is held.
	/// </summary>
	protected Transform held;

	float ghostTransparency = 0.5f;

	public Material transparentMaterial;

	public bool prebuilt = false;

	/// <summary>
	/// Initialization.
	/// </summary>
	protected virtual void Start () {
		originalMaterial = renderer.material;
		terrainLayerMask = LayerMask.NameToLayer("Environment");
		buildingLayerMask = LayerMask.NameToLayer("Obstacle");

		//This doesn't work for the build, only in the editor
		//renderer.material = new Material(renderer.material);
		//renderer.material.shader = Shader.Find("Transparent/Diffuse");

		if (!prebuilt)
		{
			renderer.material = transparentMaterial;

			float weight = 0.5f;
			red = renderer.material.color;
			red = red * weight + Color.red * weight;
			red.a = ghostTransparency;

			green = renderer.material.color;
			green = green * weight + Color.green * weight;
			green.a = ghostTransparency;

			UpdateColor();
		}
	}

	public abstract int GetOreCost();

	public abstract int GetWoodCost();

	protected abstract float HeightSpawnOffset();

	public override void SetGhostPosition(Transform heldPosition)
	{
		held = heldPosition;
		UpdatePosition();
	}

	/// <summary>
	/// Changes the color of the object & Destroys the object when health is less than or equal to zero.
	/// </summary>
	protected override void Update ()
	{
		if (held)
			UpdatePosition();

		//If the item has not been placed make it transparent and either red or green
		UpdateColor();
	}

	/// <summary>
	/// Changes the color to green for valid placement or red for invalid placement.
	/// </summary>
	protected void UpdateColor()
	{
		if (!placed)
		{
			if (CanPlace())
				renderer.material.color = green;
			else
				renderer.material.color = red;
		}
	}

	protected void UpdatePosition()
	{
		GameObject player = GameObject.FindWithTag("Player");
		Collider collider = GetComponent<Collider>();
		Vector3 position = held.position;
		if (collider && player)
		{
			//Move the building forward by half its size and upwards half its size for better positioning
			//The size of the building comes from the extents of its collider
			Vector3 direction = held.position - player.transform.position;
			direction.Normalize();
			float extents = Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.z);
			position += extents * direction;
			position += new Vector3(0f, HeightSpawnOffset(), 0f);
		}
		if (held && !placed)
			transform.position = new Vector3(Mathf.Floor(position.x / SCALE) * SCALE, position.y, Mathf.Floor(position.z / SCALE) * SCALE);
	}

	/// <summary>
	/// Attempts to place the object.
	/// <returns>True if the object was placed.</returns>
	/// </summary>
	public override bool Place()
	{
		if (CanPlace())
		{
			if (Network.connections.Length > 0)
				networkView.RPC("PlaceRPC", RPCMode.AllBuffered);
			else
				PlaceRPC();
			return true;
		}
		else
			return false;
	}

	[RPC]
	public void PlaceRPC()
	{
		placed = true;
		renderer.material = originalMaterial;
		transform.parent = null;

		collider.isTrigger = false;

		//if (rigidbody)
		//	rigidbody.isKinematic = false;
		collider.enabled = true;
	}

	/// <summary>
	/// Tests whether the item can be placed.
	/// </summary>
	/// <returns>True if the item can be placed.</returns>
	protected override bool CanPlace()
	{
		return IntersectingTerrain() && !IntersectingBuilding();
	}

	/// <summary>
	/// Called on each frame while the trigger is overlapping another collider.
	/// </summary>
	/// <param name="other">The other collider.</param>
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == terrainLayerMask)
			lastTerrainTriggerStay = DateTime.Now;
		if (other.gameObject.layer == buildingLayerMask)
			lastBuildingTriggerStay = DateTime.Now;
	}

	public bool IntersectingBuilding()
	{
		if (lastBuildingTriggerStay == null)
			return false;
		return DateTime.Now - lastBuildingTriggerStay < TimeSpan.FromSeconds(1.5f * Time.deltaTime);
	}

	/// <summary>
	/// Returns true if the building is intersecting the terrain.
	/// </summary>
	/// <returns>True if the building is grounded.</returns>
	public bool IntersectingTerrain()
	{
		if (lastTerrainTriggerStay == null)
			return false;
		return DateTime.Now - lastTerrainTriggerStay < TimeSpan.FromSeconds(1.5f * Time.deltaTime);
	}
}
