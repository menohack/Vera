using UnityEngine;
using System.Collections;

public class Campfire : Building
{
	/// <summary>
	/// The wood cost to build a wall.
	/// </summary>
	public static int CAMPFIRE_COST_WOOD = 3;

	/// <summary>
	/// The ore cost to build a wall.
	/// </summary>
	public static int CAMPFIRE_COST_ORE = 0;

	public override int GetOreCost()
	{
		return CAMPFIRE_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return CAMPFIRE_COST_WOOD;
	}

	protected override float HeightSpawnOffset()
	{
		return -2f * collider.bounds.extents.y;
	}
}
