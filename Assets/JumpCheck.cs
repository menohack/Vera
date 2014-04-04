using UnityEngine;
using System.Collections;

/// <summary>
/// This class checks
/// </summary>
public class JumpCheck : MonoBehaviour
{
	/// <summary>
	/// The layer of the environment.
	/// </summary>
	int layerMask;

	/// <summary>
	/// Counts the number of intersections with the environment. Zero means no intersection.
	/// </summary>
	int overlapCount = 0;

	/// <summary>
	/// Determines whether the object is standing on the ground or jumping/falling through the air.
	/// </summary>
	/// <returns>True if the object is grounded.</returns>
	public bool IsGrounded()
	{
		return overlapCount != 0;
	}

	void Start()
	{
		layerMask = LayerMask.NameToLayer("Environment");
	}

	/// <summary>
	/// Called when the item intersects an Environment GameObject.
	/// </summary>
	/// <param name="other">The collider that is being intersected.</param>
	void OnTriggerEnter(Collider other)
	{
		//If the item overlaps a terrain object
		if (other.gameObject.layer == layerMask)
			overlapCount++;
		Debug.Log("Derp");
	}

	/// <summary>
	/// Called when the item no longer intersects an Environment GameObject.
	/// </summary>
	/// <param name="other">The collider that is no longer being intersected.</param>
	void OnTriggerExit(Collider other)
	{
		//If the item no longer overlaps a terrain object
		if (other.gameObject.layer == layerMask)
			overlapCount--;
	}
}
