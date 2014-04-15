using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //Has many data structures

public class MeleeCollider : MonoBehaviour {

	/// <summary>
	/// The amount of damage done by the melee attack.
	/// </summary>
	public int DamageValue = 10;

	public int coolDownMilli = 1000; //TODO change this to a float and allow for 

	private TimeSpan attackCooldown;

	DateTime? lastAttack;

	void Start () {
		attackCooldown = new TimeSpan(0,0,0,0, coolDownMilli);
	}

	void Update () {
		//Hardcoded KeyCode for prelim purposes
		if (Input.GetAxis("Fire1") == 1.0f && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
		{
			transform.animation.Play();
			HashSet<GameObject> thingsWeHit = new HashSet<GameObject>(); //store each healthcomponent we hit

				
//				//Damage
//				Health myHealth = hit.GetComponent<Health>();
//				if (myHealth != null && hit != this.gameObject)
//				{
//					myHealth.Damage(DamageValue);
//				}
//			}
			
			lastAttack = DateTime.Now;
		}
	}
}
