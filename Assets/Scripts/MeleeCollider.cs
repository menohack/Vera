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

	public bool DEBUG = false;

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
			lastAttack = DateTime.Now;

		}

	}

	void OnTriggerEnter(Collider other) {
		if (this.gameObject.animation.isPlaying) {
			if (DEBUG)
				Debug.Log (gameObject.name + " just collided with " + other.gameObject.name);
			Health enemyHealth = other.gameObject.GetComponent<Health> ();
			if (enemyHealth != null) {
				if (DEBUG)
					Debug.Log (gameObject.name + " just did damage to " + other.gameObject.name);
				enemyHealth.Damage (DamageValue);
			}
	}
	}
}
