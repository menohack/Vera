using UnityEngine;
using System.Collections;

//based on GUIAspectRatioScale
public class RotateDialResize : MonoBehaviour 
{
	private RotateDial myRot;
	private Vector2 original;
	Sundial s;
	
	void Start () 
	{
		myRot = this.gameObject.GetComponent<RotateDial>();
		s = GameObject.FindObjectOfType<Sundial> ();
	}
	
	void Update()
	{
		SetAngle ();
	}

	//call on an event that tells if the aspect ratio changed
	void SetAngle()
	{
		//rotation
		myRot.angle = s.GetProgress () * 360 - 90;
	}
}