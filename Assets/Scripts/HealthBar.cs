using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;

	public float maxDistVisible = 30.0f;
	
	void OnGUI()
	{
		Bounds bounds;
		CharacterController charController = gameObject.GetComponent<CharacterController>() as CharacterController;
		if (charController != null) 
				bounds = charController.bounds;
		else
				bounds = transform.collider.bounds;
		Vector2 position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, bounds.size.y * 1.25f, 0f));
		position += new Vector2(-healthBarFront.width/2.0f, healthBarFront.height);

		Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position + new Vector3(0f, bounds.size.y * 1.25f, 0f));
		if (viewportPoint.x < 0f || viewportPoint.x > 1f || viewportPoint.y < 0f || viewportPoint.y > 1f || viewportPoint.z < 1f)
			return;

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

		float distance = Vector4.Distance(Camera.main.transform.position, transform.position);
		if (distance < maxDistVisible) 
		{
			float scale = 15f / distance;
			if (scale > 1f)
					scale = 1f;

			if (healthPercentage >= 0) 
			{
				GUI.BeginGroup (new Rect (position.x, Screen.height - position.y, healthBarBack.width * healthPercentage * scale, healthBarBack.height * scale), style);
				GUI.Box (new Rect (0, 0, healthBarBack.width * scale, healthBarBack.height * scale), healthBarBack, style);
				GUI.EndGroup ();
			}
			GUI.Box (new Rect (position.x, Screen.height - position.y, healthBarFront.width * scale, healthBarFront.height * scale), healthBarFront, style);
		}
	}
}
