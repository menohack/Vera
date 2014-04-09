using UnityEngine;

public class Wall : Building
{
	/// <summary>
	/// The wood cost to build a wall.
	/// </summary>
	public static int WALL_COST_WOOD = 1;

	/// <summary>
	/// The ore cost to build a wall.
	/// </summary>
	public static int WALL_COST_ORE = 1;

	public override int GetOreCost()
	{
		return WALL_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return WALL_COST_WOOD;
	}

	protected override void UpdatePosition()
	{
		float xOffset = 0.0f, zOffset = 0.0f;
		if (gameObject.name == "Wall 2" || gameObject.name == "WoodGate")
			xOffset = zOffset = 1.0f;
		//else if (gameObject.name == "Wall 1")
		//	xOffset = yOffset = 2.0f * Mathf.Sin(45f * Mathf.Deg2Rad);
		if (held && !placed)
			transform.position = new Vector3(Mathf.Floor(held.position.x / SCALE + 0.5f) * SCALE + xOffset, held.position.y, Mathf.Floor(held.position.z / SCALE + 0.5f) * SCALE + zOffset);
	}
}
