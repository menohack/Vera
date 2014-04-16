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

	/// <summary>
	/// Furthest distance an enemy will try to attack a player
	/// </summary>
	public float maxDist = 10f;

	public int coolDown = 3;
	protected TimeSpan attackCooldown = new TimeSpan(0, 0, 1);
	
	public Transform weapon;

	DateTime? lastAttack;

	private Transform tgt;

	void Start(){
		attackCooldown = new TimeSpan (0, 0, coolDown);
		tgt = this.gameObject.GetComponent<SeekerAI> ().target;
	}

	void Update(){
		float distFromPlayer = Vector3.Distance (tgt.position, this.gameObject.transform.position);
		if ((lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown) && (distFromPlayer < maxDist))
		{
			this.gameObject.audio.Play ();
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

			float smallestPos = 99f;
			Health closest = null;
			//Go through and apply damage to all things we hit
			foreach (GameObject hit in thingsWeHit)
			{
				bool PorB = (hit.gameObject.tag == "Building" || hit.gameObject.tag == "Player");
				if (PorB)
				{
					bool getHealth = false;
					if (Vector3.Distance(hit.transform.position, this.transform.position) < smallestPos) {
						smallestPos = Vector3.Distance(hit.transform.position, this.transform.position);
						getHealth = true;
					}
					Health myHealth = hit.GetComponent<Health>();
					//prevent from damaging self/other enemies

					if (myHealth != null && getHealth)
					{
						closest = myHealth;
					}
				}
			}
			if (closest != null)
			{ 
				closest.Damage (DamageValue);
			}
			lastAttack = DateTime.Now;
		}
	}
}