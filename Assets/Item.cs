using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
	/// <summary>
	/// Whether this item has been placed yet.
	/// </summary>
	bool placed = false;

	/// <summary>
	/// The position of the item as it is being held.
	/// </summary>
	protected GameObject floatPoint;

	public void SetFloatPoint(Transform parent, Vector3 position, Quaternion rotation)
	{
		//This should never be null. This is absurd.
		if (floatPoint == null)
			floatPoint = new GameObject("FloatPoint");
		floatPoint.transform.parent = parent;
		floatPoint.transform.localPosition = position;
		floatPoint.transform.localRotation = rotation;
		transform.parent = floatPoint.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
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
			if (floatPoint != null)
				Destroy(floatPoint);
			transform.parent = null;
			collider.isTrigger = false;
			return true;
		}
		else
			return false;
	}

	void OnDestroy()
	{
		Destroy(floatPoint);
	}

	/// <summary>
	/// Tests whether the item can be placed. Needs to be written.
	/// </summary>
	/// <returns>True if the item can be placed.</returns>
	protected virtual bool CanPlace()
	{
		return true;
	}

	public virtual string Serialize()
	{
		return gameObject.name + " " + gameObject.transform.position + " " + gameObject.transform.rotation;
	}
}
