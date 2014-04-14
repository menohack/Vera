using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;

	private int daysAlive = 0;
	public bool debug;
 
	void Update()
	{
		
	}

	void DaysAlive()
	{
		daysAlive += 1;
		if (debug) { Debug.Log ("It is now day: " + daysAlive); }
	}

	void OnGUI()
	{
		float offsetFromCorner = Screen.height * 0.1f;
		Vector2 position = new Vector2(offsetFromCorner, Screen.height - offsetFromCorner);

		float maxHealth = 0;
		float health = 0;
		Health healthScript = gameObject.GetComponent<Health>();
		if (healthScript)
		{
			health = healthScript.GetHealth();
			maxHealth = healthScript.maxHealth;
		}
		else
			Debug.LogError("Health script not found");


		float healthPercentage = health / maxHealth;

		if (healthPercentage >= 0)
		{
			GUI.BeginGroup(new Rect(position.x, Screen.height - position.y, healthBarBack.width * healthPercentage, healthBarBack.height), style);
			GUI.Box(new Rect(0, 0, healthBarBack.width, healthBarBack.height), healthBarBack, style);
			GUI.EndGroup();
		}
		GUI.Box(new Rect(position.x, Screen.height - position.y, healthBarFront.width, healthBarFront.height), healthBarFront, style);
	}
}
