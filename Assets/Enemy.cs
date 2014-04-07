using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

	//public float MOVE_FORCE = 0.1f;
	public float speed = 10.0f;

	public float minFollowDistance = 2.0f;

	public float maxFollowDistance = 20.0f;

	GameObject target = null;

	public float attackDistance = 5f;

	public TimeSpan attackCooldown = new TimeSpan(0, 0, 1);

	DateTime? lastAttack = null;

	public float attackDamage = 10.0f;

	int ignoreLayerMask;

	Vector3 attackStartPoint, attackEndPoint;

	public Boolean debug = false;

	// Use this for initialization
	void Start () {
		ignoreLayerMask = ~(1 << 10 | 1 << 11);
	}

	void Update()
	{
		attackStartPoint = transform.position + new Vector3(0, 1f, 0);
		attackEndPoint = target.transform.position;
		Attack();
		
	}

	/// <summary>
	/// Attacks the current target.
	/// </summary>
	void Attack()
	{
		Vector3 debugRay = attackEndPoint - attackStartPoint;
		debugRay.Normalize();
		debugRay *= attackDistance;

		if (target != null && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
		{
			if (IsTargetVisible())
			{
				float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);
				if (distance < attackDistance)
				{
					Health health = target.GetComponent<Health>();
					if (health)
					{
						if (debug)
							Debug.DrawLine(attackStartPoint, attackEndPoint, Color.red, 1f);
						health.Damage(attackDamage);
						lastAttack = DateTime.Now;
					}
				}
				else if (debug)
					Debug.DrawLine(attackStartPoint, attackStartPoint + debugRay, Color.blue, 1f);
			}
			else if (debug)
				Debug.DrawLine(attackStartPoint, attackStartPoint + debugRay, Color.green, 1f);
		}
	}

	bool IsTargetVisible()
	{
		Vector3 ray = attackEndPoint - attackStartPoint;
		bool hit = Physics.Raycast(new Ray(attackStartPoint, ray), ray.magnitude, ignoreLayerMask);
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
