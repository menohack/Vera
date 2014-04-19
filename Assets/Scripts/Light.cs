using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

	public GameObject sun;

	void Update ()
	{
		if (sun)
		{
			Vector3 direction = sun.transform.position - Camera.main.transform.position;
			direction.Normalize();
			transform.rotation = Quaternion.LookRotation(-direction);
		}
	}
}
