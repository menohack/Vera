using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
	/// <summary>
	/// Whether this item has been placed yet.
	/// </summary>
	bool placed = false;

	/// <summary>
	/// Attempts to place the object.
	/// <returns>True if the object was placed.</returns>
	/// </summary>
	virtual public bool Place()
	{
		if (CanPlace())
		{
			placed = true;
			collider.isTrigger = false;
			return true;
		}
		else
			return false;
	}

	/// <summary>
	/// Tests whether the item can be placed.
	/// </summary>
	/// <returns>True if the item can be placed.</returns>
	protected virtual bool CanPlace()
	{
		return true;
	}
}
