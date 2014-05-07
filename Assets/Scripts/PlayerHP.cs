using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start ()
	{
		maxWidth = this.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health == null)
			health = Spawn.GetCurrentPlayer().GetComponent<Health>();
		transform.localScale = new Vector3 ( (health.GetHealth () * maxWidth / health.maxHealth) , 1, 1);
	}
}