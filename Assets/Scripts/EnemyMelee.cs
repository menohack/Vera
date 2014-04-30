using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //Has many data structures

public class EnemyMelee : MonoBehaviour {

	/// <summary>
	/// The distance of a ray in front of the character for mining and picking up objects.
	/// </summary>
	public static float RAYCAST_DISTANCE = 3.0f;
	
	/// <summary>
	/// Sphere diameter of return code
	/// </summary>
	public static float sphereRadius = 0.2f;
	
	/// <summary>
	/// The amount of damage done by the melee attack.
	/// </summary>
	public int DamageValue = 10;

	/// <summary>
	/// Draw Raycasts and tells if idling on Debug.
	/// </summary>
	public bool debug = false;

	/// <summary>
	/// Furthest distance an enemy will try to attack a player
	/// </summary>
	public float maxDist = 10f;

	/// <summary>
	/// Cooldown time in seconds
	/// </summary>
	public int coolDown = 3;
	protected TimeSpan attackCooldown = new TimeSpan(0, 0, 1);

	DateTime? lastAttack;
	private Transform tgt;

	/// <summary>
	/// Max idle time before attacking.
	/// </summary>
	public float idleAttackTime = 1.5f;

	/// <summary>
	/// Minimum distance for being "idle" per frame.
	/// </summary>
	public float minIdleDist = 0.05f;

	SeekerAI seeker;
	
	private Vector3 lastPos;
	private float timeSinceLastMovement = 0f;

	DateTime lastTargetSearch;
	float targetSearchFrequencyMillis = 500f;
	TimeSpan targetSearchFrequency;

	void Start(){
		attackCooldown = new TimeSpan (0, 0, coolDown);
		seeker = GetComponent<SeekerAI>();
		FindTarget();
		lastPos = this.gameObject.transform.position;
		targetSearchFrequency = TimeSpan.FromMilliseconds(targetSearchFrequencyMillis);
	}

	void FindTarget()
	{
		tgt = Targeting.FindClosestTarget(transform, "Player").transform;
		if (seeker)
			seeker.target = tgt;
		lastTargetSearch = DateTime.Now;
	}

	void Update(){
		if (lastTargetSearch == null || DateTime.Now - lastTargetSearch > targetSearchFrequency)
			FindTarget();

		float distFromPlayer = Vector3.Distance (tgt.position, this.gameObject.transform.position);
		//when far away and has not moved enough, start the idle counter
		if (Vector3.Distance(lastPos, this.gameObject.transform.position) <  minIdleDist && distFromPlayer > maxDist)
		{
			timeSinceLastMovement += Time.deltaTime;
			if (debug)
			{
				Debug.Log("Idling for: " + timeSinceLastMovement); 
			}
			if (timeSinceLastMovement > idleAttackTime)
			{
				Attack ();
				timeSinceLastMovement = 0f;
			}
		}
		else { timeSinceLastMovement = 0f; }
		lastPos = transform.position;

		//when in range
		if ((lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown) && (distFromPlayer < maxDist))
		{
			Attack ();
		}
	}

	void Attack() {
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