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

	protected override float HeightSpawnOffset()
	{
		return -1.2f * collider.bounds.extents.y;
	}
}
