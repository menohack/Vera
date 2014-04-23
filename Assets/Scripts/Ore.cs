using UnityEngine;
using System.Collections;

public class Ore : Resource
{
	public int pickaxeHits = 5;
	public int oreGatherCount = 5;
	void Start()
	{
		hits = pickaxeHits;
		gatherCount = oreGatherCount;
	}
}
