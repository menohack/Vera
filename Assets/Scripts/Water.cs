using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	GameObject player;
	MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in players)
		{
			if (p.networkView.isMine)
			{
				player = p;
				break;
			}
		}
		meshFilter = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player && meshFilter && player.transform.position.y < meshFilter.transform.position.y)
		{
			Player playerScript = player.GetComponent<Player>();
			if (playerScript != null && playerScript.Alive())
				playerScript.Murder();
		}
	}
}
