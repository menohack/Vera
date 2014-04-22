using UnityEngine;
using System.Collections;

//based on GUIAspectRatioScale
public class RotateDialResize : MonoBehaviour 
{
	public Vector2 scaleOnRatio1 = new Vector2(0.1f, 0.1f);
	private float widthHeightRatio;
	private RotateDial myRot;
	private Vector2 original;
	float orX;
	float orY;
	Sundial s;
	
	void Start () 
	{
		myRot = this.gameObject.GetComponent<RotateDial>();
		original = new Vector2 (myRot.size.x, myRot.size.y);
		orX = transform.localPosition.x;
		orY = transform.localPosition.y;
		s = GameObject.FindObjectOfType<Sundial> ();
		SetScale();
	}
	
	void Update()
	{
		SetScale ();
	}

	//call on an event that tells if the aspect ratio changed
	void SetScale()
	{
		//find the aspect ratio
		widthHeightRatio = (float)Screen.width/Screen.height;
		
		//Apply the scale.
		myRot.size.x = scaleOnRatio1.x * original.x * widthHeightRatio;
		myRot.size.y = scaleOnRatio1.y * original.y * widthHeightRatio;
		transform.localPosition = new Vector3 (orX * scaleOnRatio1.x * widthHeightRatio, orY * scaleOnRatio1.y * widthHeightRatio, 0);
//		myRot.size.y = widthHeightRatio * scaleOnRatio1.y * original.y;

		//rotation
		myRot.angle = s.GetProgress () * 360 - 90;
	}
}