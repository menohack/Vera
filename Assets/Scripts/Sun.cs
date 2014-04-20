using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	public Sundial sundial;

	public float orbitalRadius = 1000f;
	public float scaleRatio = 0.1f;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(orbitalRadius * scaleRatio, orbitalRadius * scaleRatio, orbitalRadius * scaleRatio);
	}
	
	// Update is called once per frame
	void Update () {
		if (sundial)
		{
			float progress = sundial.GetProgress();
			Vector3 center = Camera.main.transform.position;
			transform.position = center + new Vector3(Mathf.Cos(2f * Mathf.PI * progress) * orbitalRadius, Mathf.Sin(2f * Mathf.PI * progress) * orbitalRadius, 0f);
		}
	}
}
