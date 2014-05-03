using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour {

	private Health p_health;
	private float maxWidth;

	// Use this for initialization
	void Start () {
		maxWidth = this.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		p_health = Spawn.GetMyPlayer().GetComponent<Health>() ;
		transform.localScale = new Vector3 ( (p_health.GetHealth () * maxWidth / p_health.maxHealth) , 1, 1);
	}
}