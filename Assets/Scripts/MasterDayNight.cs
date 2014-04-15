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
	
	private object[] lights;
	private object[] spawners;
	private object[] players;
	public AudioSource howl;

	/// <summary>
	/// The length of the day in seconds
	/// </summary>
	public float dayLength = 100;
	
	public bool debug;

	void Start() 
	{
		//if we implement multiplayer, when a new player comes in,
		//have them send a message to master knowing that so it can then update
		//the days alive of that player
		lights = GameObject.FindObjectsOfType<DayLight> ();
		spawners = GameObject.FindObjectsOfType<Spawn>();
		players = GameObject.FindObjectsOfType<Player> ();
	}

	void Update () {
		oldTime = timer - Mathf.Floor (timer);
		timer += Time.deltaTime / dayLength;
		theTime = timer - Mathf.Floor (timer);


		if (oldTime > theTime) 
		{
			if (debug) { Debug.Log ("Midnight"); }
			daySent = false; 
			nightSent = false;
		}

		if (theTime > Sunrise && !daySent)
		{
			if (debug) { Debug.Log ("Sunrise"); }
			object[] wolves = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (object o in wolves){ Destroy((GameObject) o); }
			daySent = true;
			daysAlive(players);
//			resourceSpawn(spawners);
		}
		if (theTime > Sunset && !nightSent) 
		{
			if (debug) { Debug.Log("Sunset"); }
			nightSent = true;
			wolves(spawners);
			howl.audio.Play ();
		}
		updateLights (lights);
	}

	/// <summary>
	/// Sends message to update resources (TODO)
	/// </summary>
	/// <param name="a">The alpha component.</param>
	void resourceSpawn(object[] a)
	{
		foreach (object o in a)
		{
			((GameObject) o).SendMessage("SpawnWorld", SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Sends message to update lights.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	void updateLights(object[] a)
	{
		foreach (object o in a)
		{
			((DayLight) o).SendMessage("lightColors", theTime, SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Sends message to spawn wolves.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	void wolves(object[] a)
	{
		foreach (object o in a)
		{
			((Spawn) o).SendMessage ("SpawnWolves", SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Updates days alive for players.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	void daysAlive(object[] a)
	{
		foreach (object o in a)
		{
			((Player) o).SendMessage ("DaysAlive", SendMessageOptions.DontRequireReceiver);
		}
	}
}
