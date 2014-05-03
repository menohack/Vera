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
		FindPlayer();
		meshFilter = GetComponent<MeshFilter>();
	}

	void FindPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in players)
		{
			if (p.networkView.isMine)
			{
				player = p;
				break;
			}
		}
		lastSearchTime = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player == null && DateTime.Now - lastSearchTime > searchFrequency)
			FindPlayer();

		if (player != null && meshFilter != null && player.transform.position.y < meshFilter.transform.position.y)
		{
			Player playerScript = player.GetComponent<Player>();
			if (playerScript != null && playerScript.Alive())
				playerScript.Murder();
		}
	}
}
