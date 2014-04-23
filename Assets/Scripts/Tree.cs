using UnityEngine;
using System.Collections;

public class Tree : Resource {

	public int axeHits = 3;
	public int woodGatherCount = 3;

	void Start()
	{
		hits = axeHits;
		gatherCount = woodGatherCount;
	}
}
