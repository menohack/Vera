using UnityEngine;

public class Gate : Building
{
	/// <summary>
	/// The wood cost to build a gate.
	/// </summary>
	public static int GATE_COST_WOOD = 1;

	/// <summary>
	/// The ore cost to build a gate.
	/// </summary>
	public static int GATE_COST_ORE = 1;

	public override int GetOreCost()
	{
		return GATE_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return GATE_COST_WOOD;
	}

	protected override float HeightSpawnOffset()
	{
		return -0.9f * collider.bounds.extents.y;
	}
}
