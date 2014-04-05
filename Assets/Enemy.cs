using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//public float MOVE_FORCE = 0.1f;
	public float speed = 10.0f;

	public float minFollowDistance = 2.0f;

	public float maxFollowDistance = 20.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
		
		if (targets.Length > 0)
		{
			GameObject target = targets[0];
			float minDistance = Vector3.Distance(targets[0].gameObject.transform.position, transform.position);
			for (int i=1; i < targets.Length; i++)
			{
				float distance = Vector3.Distance(targets[i].gameObject.transform.position, transform.position);
				if (distance < minDistance )
				{
					minDistance = distance;
					target = targets[i];
				}
			}

			
			transform.LookAt(target.transform.position, Vector3.up);
			if (minDistance < maxFollowDistance && minDistance > minFollowDistance)
				//rigidbody.AddForce(transform.forward * MOVE_FORCE * Time.fixedDeltaTime, ForceMode.VelocityChange);
				transform.position += transform.forward * Time.fixedDeltaTime * speed;
			
		}
			
	}
}
