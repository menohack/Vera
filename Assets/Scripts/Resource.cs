using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {

	/// <summary>
	/// The number of axe or pickaxe hits to gather.
	/// </summary>
	protected int hits;

	/// <summary>
	/// The number of units of the resource per gather.
	/// </summary>
	protected int gatherCount;

	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Attempts to gather a resource.
	/// </summary>
	/// <param name="numHits">The number of axe or pickaxe hits.</param>
	/// <returns>The number of units gathered.</returns>
	public int Gather(int numHits)
	{
		if (numHits <= 0)
			throw new UnityException("numHits must be positive");

		hits -= numHits;
		if (hits <= 0)
		{
			hits = 0;
			Utility.DestroyHelper(gameObject);
			return gatherCount;
		}
		else
			return 0;
	}
}
