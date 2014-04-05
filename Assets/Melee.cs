using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Has many data structures

public class Melee : MonoBehaviour {

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
	/// Force applied to a hit in "Newtons."
	/// </summary>
	public float myForce = 500f;

	/// <summary>
	/// Draw Raycasts on Debug.
	/// </summary>
	public bool debug = false;
	
	void Update () {
		//Hardcoded KeyCode for prelim purposes
		if (Input.GetKeyDown(KeyCode.V))
		{
			HashSet<GameObject> thingsWeHit = new HashSet<GameObject>(); //store each healthcomponent we hit

			for (int i = 0; i < 5; i++)
			{
				//Create a ray out of the player in a certain spherical radius (in lieu of cone gameObject)
				Vector3 myDir = transform.forward;
				myDir += Random.insideUnitSphere * sphereRadius; //gives new direction
				myDir.Normalize(); //set magnitude of vector back to 1

				//get all hits in the ray
				RaycastHit[] hits = Physics.RaycastAll(transform.position, myDir, RAYCAST_DISTANCE);
				if (debug)
				{
					Debug.DrawLine(transform.position, transform.position + (myDir * RAYCAST_DISTANCE), Color.green);
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
				//Knockback
				Rigidbody myBodyIsRigid = hit.GetComponent<Rigidbody>();
				if (myBodyIsRigid != null)
				{
					Vector3 displace = hit.transform.position - transform.position;
					displace.Normalize();
					myBodyIsRigid.AddForce(displace * myForce);
				}

				//Damage
				Health myHealth = hit.GetComponent<Health>();
				if (myHealth != null)
				{
					myHealth.Damage(DamageValue);
				}
			}

		}
	}
}
