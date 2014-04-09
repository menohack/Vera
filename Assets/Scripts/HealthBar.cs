using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;

	public float DISPLAY_RANGE = 100;
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Returns true if the health bar should be visible to the main camera.
	/// TODO: Make it based on angle so enemies behind the camera do not show.
	/// </summary>
	/// <returns>True if visible.</returns>
	bool IsVisible()
	{
		return Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) < DISPLAY_RANGE;
	}

	void OnGUI()
	{
		if (!IsVisible())
			return;
		BoxCollider boxCollider = transform.collider as BoxCollider;
		Vector2 position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, boxCollider.size.y * 1.25f, 0f));
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
