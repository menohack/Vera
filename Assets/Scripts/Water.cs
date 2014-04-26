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
	void Update ()
	{
		if (player && player.transform.position.y < meshFilter.transform.position.y)
		{
			player.rigidbody.AddForce(Vector3.up * 1000f * Time.deltaTime);
		}
	}
}
