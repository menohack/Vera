using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
	/// <summary>
	/// Whether this item has been placed yet.
	/// </summary>
	protected bool placed = false;

	public virtual void SetGhostPosition(Transform heldPosition)
	{
		return;
	}

	/// <summary>
	/// Attempts to place the object.
	/// <returns>True if the object was placed.</returns>
	/// </summary>
	virtual public bool Place()
	{
		if (CanPlace())
		{
			placed = true;
			transform.parent = null;
			collider.isTrigger = false;
			return true;
		}
		else
			return false;
	}

	/// <summary>
	/// Tests whether the item can be placed. Needs to be written.
	/// </summary>
	/// <returns>True if the item can be placed.</returns>
	protected virtual bool CanPlace()
	{
		throw new UnityException("Not implemented");
	}

	public virtual string Serialize()
	{
		return gameObject.name + " " + gameObject.transform.position + " " + gameObject.transform.rotation;
	}

	protected virtual void Update()
	{
	}
}
