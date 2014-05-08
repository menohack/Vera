using System;
using UnityEngine;

/// <summary>
/// The JumpCheck class is a trigger that checks for collision with the ground to enable jumping.
/// </summary>
public class JumpCheck : MonoBehaviour
{
	/// <summary>
	/// The layer mask for things from which we can jump.
	/// </summary>
	int layerMask;

	/// <summary>
	/// The last time that OnTriggerStay was called.
	/// </summary>
	DateTime? lastTriggerStay = null;

	/// <summary>
	/// Set the mask of layers on which we can jump.
	/// </summary>
	void Start()
	{
		layerMask = 1 << LayerMask.NameToLayer("Environment") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy");
	}

	/// <summary>
	/// Determines whether the object is standing on the ground or jumping/falling through the air.
	/// </summary>
	/// <returns>True if the object is grounded.</returns>
	public bool IsGrounded()
	{
		if (lastTriggerStay == null)
			return false;
		return DateTime.Now - lastTriggerStay < TimeSpan.FromSeconds(1.5f * Time.deltaTime);
	}

	/// <summary>
	/// Called on each frame while the trigger is overlapping another collider.
	/// </summary>
	/// <param name="other">The other collider.</param>
	void OnTriggerStay(Collider other)
	{
		if ((1 << other.gameObject.layer & layerMask) == 1 << other.gameObject.layer)
			lastTriggerStay = DateTime.Now;
	}
}
