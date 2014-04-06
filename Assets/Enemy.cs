using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

	//public float MOVE_FORCE = 0.1f;
	public float speed = 10.0f;

	public float minFollowDistance = 2.0f;

	public float maxFollowDistance = 20.0f;

	GameObject target = null;

	public float attackDistance = 12;

	public TimeSpan attackCooldown = new TimeSpan(0, 0, 1);

	DateTime lastAttack;

	public float attackDamage = 10.0f;

	int ignoreLayerMask;

	// Use this for initialization
	void Start () {
		ignoreLayerMask = ~(1 << 10 | 1 << 11);
	}

	void Update()
	{
		Attack();
		
	}

	/// <summary>
	/// Attacks the current target.
	/// </summary>
	void Attack()
	{
		if (target != null && IsTargetVisible() && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
		{
			float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);
			if (distance < attackDistance)
			{
				Health health = target.GetComponent<Health>();
				if (health)
				{
					Debug.Log("Enemy attacked");
					health.Damage(attackDamage);
					lastAttack = DateTime.Now;
				}
			}
		}
	}

	bool IsTargetVisible()
	{
		Vector3 ray = target.transform.position - transform.position;
		float length = Vector3.Distance(target.transform.position, transform.position);
		bool hit = Physics.Raycast(new Ray(transform.position, ray), length, ignoreLayerMask);
		if (!hit)
			Debug.DrawLine(transform.position, target.transform.position, Color.red, 0.5f);
		else
			Debug.DrawLine(transform.position, target.transform.position, Color.blue, 0.5f);
		return !hit;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
		
		if (targets.Length > 0)
		{
			target = targets[0];
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

			
			//transform.LookAt(target.transform.position, Vector3.up);
			if (minDistance < maxFollowDistance && minDistance > minFollowDistance)
				//rigidbody.AddForce(transform.forward * MOVE_FORCE * Time.fixedDeltaTime, ForceMode.VelocityChange);
				//transform.position += transform.forward * Time.fixedDeltaTime * speed;
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
			
		}
			
	}
}
