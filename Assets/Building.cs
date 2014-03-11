using UnityEngine;
using System.Collections;

public abstract class Building : Item {


	private float health = 100f;

	public void Damage(float dmg)
	{
		health -= dmg;
	}

	public bool IsAlive()
	{
		if (health <= 0)
			return false;
		else
			return true;
	}

	/// <summary>
	/// Counts the number of intersections with the environment. Zero means no intersection.
	/// </summary>
	protected int overlapCount = 0;

	/// <summary>
	/// Whether this item has been placed yet.
	/// </summary>
	bool placed = false;

	protected bool attached = false;

	/// <summary>
	/// The layer of the environment.
	/// </summary>
	int layerMask;

	/// <summary>
	/// The minimum distance two buildings that are not connected may be.
	/// </summary>
	public float minBuildingDistance = 10.0f;

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
	protected void Start () {
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

	public abstract void Attach();
	
	/// <summary>
	/// Changes the color of the object.
	/// </summary>
	protected void Update ()
	{
		if (!IsAlive())
			Destroy(this);

		//If the item has not been placed make it transparent and either red or green
		if (!placed)
		{
			Attach();

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
		if (attached)
			return true;

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
			if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < minBuildingDistance)
				return false;

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
