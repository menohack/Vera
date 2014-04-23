using UnityEngine;
using System.Collections;

public class Cottage : Building {

	public int COTTAGE_COST_ORE = 5;

	public int COTTAGE_COST_WOOD = 5;

	public override int GetOreCost()
	{
		return COTTAGE_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return COTTAGE_COST_WOOD;
	}
}
