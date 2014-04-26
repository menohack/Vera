using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour {

	private Health p_health;
	private float maxWidth;

	// Use this for initialization
	void Start () {
		p_health = GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>() ;
		maxWidth = this.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3 ( (p_health.GetHealth () * maxWidth / p_health.maxHealth) , 1, 1);
	}
}