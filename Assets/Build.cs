using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {

	public Transform buildObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.E))
		{
			GameObject go = Instantiate(buildObject, transform.position + transform.forward * 5.0f, transform.rotation) as GameObject;
		}
	}
}
