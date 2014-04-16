using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	GameObject player;
	MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		meshFilter = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("player: " + player.transform.position.y + ", water: " + meshFilter.transform.position.y);
		if (player && player.transform.position.y < meshFilter.transform.position.y)
		{
			Debug.Log("To the moon!");
			player.rigidbody.AddForce(Vector3.up * 1000f * Time.deltaTime);
		}
	}
}
