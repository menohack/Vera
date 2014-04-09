using UnityEngine;
using System.Collections;

public class MasterDayNight : MonoBehaviour {

	/// <summary>
	/// The master timer.
	/// </summary>
	public float timer = 0f;

	private bool daySent;
	private bool nightSent;
	private float oldTime = 0f;
	private float theTime = 0f;
	
	public static float Midnight = 0f;
	public static float Sunrise = 0.25f;
	public static float Noon = 0.5f;
	public static float Sunset = 0.75f;

	public Spawn spawn;

	/// <summary>
	/// The length of the day in seconds
	/// </summary>
	public float dayLength = 100;

	void Update () {
		oldTime = timer - Mathf.Floor (timer);
		timer += Time.deltaTime / dayLength;
		theTime = timer - Mathf.Floor (timer);

		object[] all = GameObject.FindObjectsOfType<GameObject>();
		if (oldTime > theTime) 
		{
			daySent = false; 
			nightSent = false;
		}

		if (theTime > Sunrise && !daySent)
		{
			daySent = true;
			resourceSpawn(all);
		}
		if (theTime > Sunset && !nightSent) 
		{
			nightSent = true;
			wolves(all);
		}
		updateLights (all);
	}

	void resourceSpawn(object[] a)
	{
		Debug.Log ("Resources Spawning at Day");
//		foreach (object o in a)
//		{
//			((GameObject) o).SendMessage("SpawnWorld", SendMessageOptions.DontRequireReceiver);
//		}
	}

	void updateLights(object[] a)
	{
		foreach (object o in a)
		{
			((GameObject) o).SendMessage("lightColors", theTime, SendMessageOptions.DontRequireReceiver);
		}
	}

	void wolves(object[] a)
	{
		Debug.Log ("Spawn Wolves at Night");
		spawn.SpawnWolves();
	}
}
