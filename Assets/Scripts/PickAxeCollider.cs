using UnityEngine;
using System.Collections;

/// <summary>
/// The collider for a pickaxe. A pickaxe can gather only ore and it cannot destroy structures.
/// </summary>
public class PickAxeCollider : ResourceGatherCollider
{
	/// <summary>
	/// Gets the tag that corresponds to the resource that this weapon can gather.
	/// </summary>
	/// <returns>The tag of the resource.</returns>
	protected override string GetResourceTag()
	{
		return "Ore";
	}

	/// <summary>
	/// Returns true if this weapon can destroy buildings.
	/// </summary>
	/// <returns>True if this weapon can destroy buildings.</returns>
	protected override bool CanDestroyBuilding()
	{
		return false;
	}
}
