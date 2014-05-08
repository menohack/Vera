using UnityEngine;
using System.Collections;

/// <summary>
/// The collider for an axe. An axe can gather only wood and it cannot destroy structures.
/// </summary>
public class AxeCollider : ResourceGatherCollider
{
	/// <summary>
	/// Gets the tag that corresponds to the resource that this weapon can gather.
	/// </summary>
	/// <returns>The tag of the resource.</returns>
	protected override string GetResourceTag()
	{
		return "Tree";
	}

	/// <summary>
	/// Returns true if this weapon can destroy buildings.
	/// </summary>
	/// <returns>True if this weapon can destroy buildings.</returns>
	protected override bool CanDestroyBuilding()
	{
		return true;
	}
}
