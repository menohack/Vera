using UnityEngine;
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
	public float minBuildingDistance = 1.0f;

	/// <summary>
	/// The original material of the wall.
	/// </summary>
	Material originalMaterial;

	/// <summary>
	/// The colors used for shading an unbuild building.
	/// </summary>
	Color red, green;

	public float SCALE = 2f;

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
	protected void Start () {
		originalMaterial = renderer.material;
		layerMask = LayerMask.NameToLayer("Environment");

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
		}
	}

	public abstract int GetOreCost();

	public abstract int GetWoodCost();

	public override void SetGhostPosition(Transform heldPosition)
	{
		held = heldPosition;
		UpdatePosition();
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
		if (held)
			UpdatePosition();

		//If the item has not been placed make it transparent and either red or green
		if (!placed)
		{
			if (CanPlace())
				renderer.material.color = green;
			else
				renderer.material.color = red;
		}
	}

	//protected abstract void UpdatePosition();

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
			position -= new Vector3(0f, collider.bounds.extents.y, 0f);
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

		//This is broken, no idea why
		return true;
//		return IntersectingTerrain();
	}

	protected bool IntersectingTerrain()
	{
		return overlapCount != 0;
	}

	void OnGUI()
	{
		/*
		GUIStyle style = new GUIStyle();
		style.fontSize = 32;
		GUI.Label(new Rect(Screen.width - 200, 200, 200, 200), "overlap: " + overlapCount, style);
		 * */
	}

	/// <summary>
	/// Called when the item intersects an Environment GameObject.
	/// </summary>
	/// <param name="other">The collider that is being intersected.</param>
	void OnTriggerEnter(Collider other)
	{
		//Debug.Log(overlapCount);
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
		//Debug.Log(overlapCount);
		//If the item no longer overlaps a terrain object
		if (other.gameObject.layer == layerMask)
			overlapCount--;
	}
}
