using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {

	GameObject player;
	MeshFilter meshFilter;

	DateTime lastSearchTime;
	TimeSpan searchFrequency;
	float searchFrequencyMillis = 500f;

	// Use this for initialization
	void Start () {
		searchFrequency = TimeSpan.FromMilliseconds(searchFrequencyMillis);
		meshFilter = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		player = Spawn.GetCurrentPlayer();

		if (player != null && meshFilter != null && player.transform.position.y < meshFilter.transform.position.y)
		{
			Player playerScript = player.GetComponent<Player>();
			if (playerScript != null && playerScript.Alive())
				playerScript.Murder();
		}
	}
}
