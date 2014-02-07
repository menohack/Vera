using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public float speed = 5.0f;
	public float turnSpeed = 90.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 9.8f;
	float vSpeed = 0;
 
	void Update()
	{
		transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
		Vector3 vel = transform.forward * Input.GetAxis("Vertical") * speed;
		CharacterController controller = GetComponent<CharacterController>() as CharacterController;

		if (controller.isGrounded)
		{
			vSpeed = 0; // grounded character has vSpeed = 0...
			if (Input.GetKeyDown("space")){ // unless it jumps:
				vSpeed = jumpSpeed;
			}
		}
		// apply gravity acceleration to vertical speed:
		vSpeed -= gravity * Time.deltaTime;
		vel.y = vSpeed; // include vertical speed in vel
		// convert vel to displacement and Move the character:
		controller.Move(vel * Time.deltaTime);
	}
}
