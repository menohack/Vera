using System;
/// <summary>
/// This class checks
/// </summary>
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
	/// <summary>
	/// The layer of the environment.
	/// </summary>
	int layerMask;

	/// <summary>
	/// The last time that OnTriggerStay was called.
	/// </summary>
	DateTime? lastTriggerStay = null;

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

	void Start()
	{
		layerMask = LayerMask.NameToLayer("Environment") | LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Enemy");
	}

	/// <summary>
	/// Called on each frame while the trigger is overlapping another collider.
	/// </summary>
	/// <param name="other">The other collider.</param>
	void OnTriggerStay(Collider other)
	{
		if ((other.gameObject.layer & layerMask) == other.gameObject.layer)
			lastTriggerStay = DateTime.Now;
	}
}
