﻿using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;

	bool alive;

	DateTime deathTime;
	float deathCameraTimeMillis = 5000f;
	TimeSpan deathCameraTime;

	public Texture2D deathTexture;

	void Start()
	{
		Screen.showCursor = false;
		deathCameraTime = TimeSpan.FromMilliseconds(deathCameraTimeMillis);
		alive = true;
	}

	void Update()
	{
		if (Input.GetButtonDown("Escape") && !Menu.GameOver())
		{
			if (Menu.Paused())
			{
				if (Network.isServer)
					networkView.RPC("Resume", RPCMode.AllBuffered);
				else if (Network.connections.Length == 0)
					Menu.Resume();
			}
			else
			{
				if (Network.isServer)
					networkView.RPC("Pause", RPCMode.AllBuffered);
				else if (Network.connections.Length == 0)
					Menu.Pause();
			}
		}

		if (!alive && deathTime != null && DateTime.Now - deathTime > deathCameraTime)
		{
			alive = true;
			Spawn spawn = FindObjectOfType<Spawn>();
			if (spawn)
			{
				spawn.RespawnPlayer();
				Utility.DestroyHelper(gameObject);
			}
			else
				Debug.Log("Unable to find Spawn in Player");
		}
	}

	void OnGUI()
	{
		GUI.depth = 5;
		if (!alive)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), deathTexture, ScaleMode.StretchToFill);
		}
	}

	/// <summary>
	/// Returns true if the player is alive.
	/// </summary>
	/// <returns>True if the player is alive.</returns>
	public bool Alive()
	{
		return alive;
	}

	/// <summary>
	/// Murders the player. Gruesomely.
	/// </summary>
	public void Murder()
	{
		alive = false;
		rigidbody.isKinematic = false;

		//Destroy the input controllers
		ThirdPersonController controller = GetComponent<ThirdPersonController>();
		if (controller)
			Destroy(controller);
		MouseLook mouseLook = GetComponent<MouseLook>();
		if (mouseLook)
			Destroy(mouseLook);
		mouseLook = GetComponentInChildren<MouseLook>();
		if (mouseLook)
			Destroy(mouseLook);

		//And lock the camera
		Transform child = transform.FindChild("CamLookPoint");
		if (child)
		{
			child = child.FindChild("Main Camera");
			if (child)
			{
				child.parent = null;
				AudioListener listener = child.gameObject.GetComponent<AudioListener>();
				if (listener)
					Destroy(listener);
				else
					Debug.Log("Could not find AudioListener on Main Camera");
			}
			else
				Debug.Log("Could not find Main Camera in player");
		}
		else
			Debug.Log("Could not find CamLookPoint");

		deathTime = DateTime.Now;
	}

	/// <summary>
	/// Remove keyboard and mouse controls locally from other players.
	/// </summary>
	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		ThirdPersonController controller = gameObject.GetComponent<ThirdPersonController>();
		if (controller != null && !networkView.isMine)
		{
			Destroy(gameObject.GetComponent<ThirdPersonController>());
			Destroy(gameObject.GetComponent<MouseLook>());
			Transform t = gameObject.transform.FindChild("CamLookPoint");
			if (t != null)
				Destroy(t.gameObject);
			Destroy(gameObject.GetComponent<Build>());
			MeleeCollider[] mcs = FindObjectsOfType<MeleeCollider>();
			foreach (MeleeCollider m in mcs)
			{
				Transform temp = m.transform;
				while (temp.parent != null)
					temp = temp.parent;
				if (temp.networkView && !temp.networkView.isMine)
					Destroy(m);
			}
			gameObject.rigidbody.isKinematic = true;
			Destroy(this);
		}
	}
}
