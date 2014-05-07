using UnityEngine;
using System;

/// <summary>
/// MeleeCollider is placed on a weapon GameObject and handles damage to enemies from collision.
/// </summary>
public class MeleeCollider : MonoBehaviour {

	public bool DEBUG = false;

	/// <summary>
	/// The amount of damage done by the melee attack.
	/// </summary>
	public int DamageValue = 10;

	/// <summary>
	/// The attack cooldown in milliseconds.
	/// </summary>
	public float coolDownMillis = 1000f;

	/// <summary>
	/// The attack cooldown as a TimeSpan.
	/// </summary>
	private TimeSpan attackCooldown;

	/// <summary>
	/// The time of the last attack.
	/// </summary>
	DateTime? lastAttack;

	/// <summary>
	/// The animation of the player attacking.
	/// </summary>
	public PlayerAnimation anime;

	void Start ()
	{
		attackCooldown = TimeSpan.FromMilliseconds(coolDownMillis);
	}

	void Update ()
	{
		GameObject player = Spawn.GetCurrentPlayer();
		if (player != null) 
		{
			Build buildScript = player.GetComponent<Build>();
			if (buildScript && !buildScript.HasBuilding())
			{
				if (Input.GetButtonDown("Fire1") && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
				{
					if (anime)
						anime.Attack();
					else
						Debug.Log("Animation missing from Player");
					lastAttack = DateTime.Now;
				}
			}
		}
	}

	/// <summary>
	/// Inflicts damage when the weapon collides with an enemy.
	/// </summary>
	/// <param name="other"></param>
	void OnTriggerEnter(Collider other) {
		if (anime != null && anime.GetAttacking()) {

			Health enemyHealth = null;
			if (other.gameObject.tag == "Enemy")
			{
				if (DEBUG) PrintCollided();
				enemyHealth = other.gameObject.GetComponent<Health> ();
			}
			if (enemyHealth != null) {
				enemyHealth.Damage (DamageValue);
				if (DEBUG) PrintSuccess();
			}
		}
	}

	private void PrintCollided() {
		Debug.Log ("Enemy collided with");
	}
	private void PrintSuccess() {
		Debug.Log ("Enemy succesfully attacked");
	}
}
