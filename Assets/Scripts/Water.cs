using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The Water class kills the player when he touches water.
/// </summary>
public class Water : MonoBehaviour
{
	/// <summary>
	/// The water mesh.
	/// </summary>
	MeshFilter meshFilter;

	/// <summary>
	/// Gets the water mesh.
	/// </summary>
	void Start ()
	{
		meshFilter = GetComponent<MeshFilter>();
	}
	
	/// <summary>
	/// Brutally murders the player if his height is below the water height.
	/// </summary>
	void Update ()
	{
		GameObject player = Spawn.GetCurrentPlayer();

		if (player != null && meshFilter != null && player.transform.position.y < meshFilter.transform.position.y)
		{
			Player playerScript = player.GetComponent<Player>();
			if (playerScript != null && playerScript.Alive())
				playerScript.Murder();
		}
	}
}
