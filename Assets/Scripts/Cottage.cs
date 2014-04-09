using UnityEngine;
using System.Collections;

public class Cottage : Building {

	public static int COTTAGE_COST_ORE = 5;

	public static int COTTAGE_COST_WOOD = 5;

	public override int GetOreCost()
	{
		return COTTAGE_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return COTTAGE_COST_WOOD;
	}

	protected override void UpdatePosition()
	{
		float xOffset = 0.0f, zOffset = 5.0f;
		if (held && !placed)
			transform.position = new Vector3(Mathf.Floor(held.position.x / SCALE + 0.5f) * SCALE + xOffset, held.position.y, Mathf.Floor(held.position.z / SCALE + 0.5f) * SCALE + zOffset);
	}
}
