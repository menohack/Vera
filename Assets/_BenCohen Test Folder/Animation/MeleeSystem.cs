using UnityEngine;
using System.Collections;

public class MeleeSystem : MonoBehaviour {

	public int damage = 50;
	public float maxDistance = 1.5f;
	public Transform weapon;
	private float distance;


	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			weapon.animation.Play("MaceAttack");
			performAttack ();
		}
	
	}

	void performAttack () {
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, maxDistance)) {
			//Debug.Log (gameObject + " sent message: \"Apply Damage\"");
			hit.transform.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

	}
}
