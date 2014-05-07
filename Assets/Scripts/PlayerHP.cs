using UnityEngine;
using System.Collections;

/// <summary>
/// Changes the health bar size on the GUI.
/// </summary>
public class PlayerHP : MonoBehaviour
{
	/// <summary>
	/// The Health script of the current player.
	/// </summary>
	Health health = null;

	/// <summary>
	/// The max width of the player's health bar.
	/// </summary>
	private float maxWidth;

	void Start ()
	{
		maxWidth = this.transform.localScale.y;
	}
	
	void Update ()
	{
		if (health == null)
			health = Spawn.GetCurrentPlayer().GetComponent<Health>();
		transform.localScale = new Vector3 ( (health.GetHealth () * maxWidth / health.maxHealth) , 1, 1);
	}
}