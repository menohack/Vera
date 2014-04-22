﻿using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;
	
	void OnGUI()
	{
		Bounds bounds;
		CharacterController charController = gameObject.GetComponent<CharacterController>() as CharacterController;
		if (charController != null) 
				bounds = charController.bounds;
		else
				bounds = transform.collider.bounds;
		Vector2 position = Camera.main.WorldToViewportPoint(transform.position + new Vector3(0f, bounds.size.y * 1.25f, 0f));
		position += new Vector2(-healthBarFront.width/2.0f, healthBarFront.height);

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
