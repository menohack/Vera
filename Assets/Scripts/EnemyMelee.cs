using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //Has many data structures

public class EnemyMelee : MonoBehaviour {

	/// <summary>
	/// The distance of a ray in front of the character for mining and picking up objects.
	/// </summary>
	public static float RAYCAST_DISTANCE = 5.0f;
	
	/// <summary>
	/// Sphere diameter of return code
	/// </summary>
	public static float sphereRadius = 0.2f;
	
	/// <summary>
	/// The amount of damage done by the melee attack.
	/// </summary>
	public int DamageValue = 10;

	/// <summary>
	/// Draw Raycasts on Debug.
	/// </summary>
	public bool debug = false;

	public int coolDown = 1;
	protected TimeSpan attackCooldown = new TimeSpan(0, 0, 1);
	
	public Transform weapon;
	
	DateTime? lastAttack;

	void Start(){
		attackCooldown = new TimeSpan (0, 0, coolDown);
	}

	void Update(){
		if (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown)
		{
	//		weapon.animation.Play();
			HashSet<GameObject> thingsWeHit = new HashSet<GameObject>();
			
			for (int i = 0; i < 5; i++)
			{
				//Create a ray out of the player in a certain spherical radius (in lieu of cone gameObject)
				Vector3 myDir = transform.forward;
				myDir += UnityEngine.Random.insideUnitSphere * sphereRadius; //gives new direction
				myDir.Normalize(); //set magnitude of vector back to 1

				Vector3 attackPos = transform.position + Vector3.up;
				RaycastHit[] hits = Physics.RaycastAll(attackPos, myDir, RAYCAST_DISTANCE);
				if (debug)
				{
					Debug.DrawLine(attackPos, attackPos + (myDir * RAYCAST_DISTANCE), Color.green);
				}
				
				//for each thing we hit, add it to our hash if it isn't already there
				foreach (RaycastHit objHit in hits)
				{
					GameObject obj = objHit.transform.gameObject;
					if (!thingsWeHit.Contains(obj))
					{
						thingsWeHit.Add(obj);
					}
				}
			}
			
			//Go through and apply damage to all things we hit
			foreach (GameObject hit in thingsWeHit)
			{
				
				//Damage
				Health myHealth = hit.GetComponent<Health>();
				if (myHealth != null && hit != this.gameObject)
				{
					myHealth.Damage(DamageValue);
				}
			}
			lastAttack = DateTime.Now;
		}
	}
}