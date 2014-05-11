using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode()] 
public class TitleScreen : MonoBehaviour {

	public Texture2D titleScreen, singleplayerScreen, multiplayerScreen, infoScreen, controlScreen;

	bool loading = false, controls = false, info = false;

	Texture2D texture;

	NetworkController nc;

	public Font font;

	string ipFieldString = "Server IP";

	public Texture2D cursorTexture;

	Vector3 mouseStart;

	void Start()
	{
		texture = titleScreen;
		
		Screen.showCursor = true;
		Screen.lockCursor = false;

		nc = FindObjectOfType<NetworkController>();

		mouseStart = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
		scrollLength = TimeSpan.FromMilliseconds(scrollLengthMillis);
	}

	void Update()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	[RPC]
	public void StartGame()
	{
		loading = true;
		Application.LoadLevel("Level1");
	}

	const int MAX_SCREEN_OFFSET = 600;
	const float SCREEN_MOVE_SCALE = 2f;

	float Spline(float t)
	{
		return t * t * t * Screen.width;
	}

	enum ScreenState
	{
		Singleplayer,
		ScrollLeft,
		Title,
		ScrollRight,
		Multiplayer
	}

	ScreenState state = ScreenState.Title;
	DateTime scrollStart;
	public float scrollLengthMillis = 250f;
	TimeSpan scrollLength;

	public float swipeSpeed = 1000f;

	float swipeT;

	float ComputeAutoScreenOffset(float t)
	{
		return t;
	}

	float screenOffset = 0f;

	void OnGUI()
	{
	
		float mouseDelta = mouseStart.x - Input.mousePosition.x;
		float t = mouseDelta / (Screen.width / 4.0f);
		
		if (state == ScreenState.ScrollLeft)
		{
			float temp = (float)((DateTime.Now - scrollStart).TotalMilliseconds) / scrollLengthMillis;
			if (temp > 1.0f)
			{
				state = ScreenState.Singleplayer;
				screenOffset = -Screen.width;
			}
			else
				;
		}
		else
		{
			if (t > 1.0f)
			{
				state = ScreenState.ScrollLeft;
			}
			else
				screenOffset = Mathf.Clamp(Spline(t), -1.0f, 1.0f);
		}

		GUI.DrawTexture(new Rect(screenOffset, 0, Screen.width, Screen.height), texture, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture(new Rect(screenOffset - Screen.width, 0, Screen.width, Screen.height), singleplayerScreen, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture(new Rect(screenOffset + Screen.width, 0, Screen.width, Screen.height), multiplayerScreen, ScaleMode.ScaleAndCrop);
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.font = font;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 32;
		labelStyle.alignment = TextAnchor.UpperCenter;

		if (loading)
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height * 0.9f, 200, 100), "LOADING", labelStyle);

		if (!loading)
		{
			if ((!nc.Connected() || Network.isServer) && GUI.Button(new Rect(screenOffset + Screen.width / 2 - 50, Screen.height * 0.9f, 100, 40), "Start Game", labelStyle))
			{
				if (Network.isServer)
					networkView.RPC("StartGame", RPCMode.AllBuffered);
				else
					StartGame();
			}
			if (!controls && GUI.Button(new Rect(screenOffset + Screen.width / 2 - 150, Screen.height * 0.9f, 100, 40), "Controls", labelStyle))
			{
				controls = true;
				info = false;
				texture = controlScreen;
			}
			if (!info && GUI.Button(new Rect(screenOffset + Screen.width / 2 + 50, Screen.height * 0.9f, 100, 40), "Info", labelStyle))
			{
				info = true;
				controls = false;
				texture = infoScreen;
			}
			if (GUI.Button(new Rect(screenOffset + Screen.width / 2 - 50, Screen.height * 0.9f + 40, 100, 40), "Quit", labelStyle))
				Application.Quit();
		}



		if (nc != null && !nc.Connected() && !Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(screenOffset + Screen.width + 100, 100, 250, 100), "Start Server"))
				nc.StartServer();

			if (GUI.Button(new Rect(screenOffset + Screen.width + 100, 250, 250, 100), "Refresh Hosts"))
				nc.RefreshHostList();

			ipFieldString = GUI.TextField(new Rect(screenOffset + Screen.width + 100, 400, 250, 50), ipFieldString);
			if (GUI.Button(new Rect(screenOffset + Screen.width + 350, 400, 80, 50), "Connect"))
 				nc.JoinServer(ipFieldString);

			if (nc.hostList != null)
			{
				GUI.BeginScrollView(new Rect(screenOffset + Screen.width + 400, 100, 300, 400), Vector2.zero, new Rect(0, 0, 300, 300));
				for (int i = 0; i < nc.hostList.Length; i++)
				{
					if (GUI.Button(new Rect(0, 110 * i, 300, 100), nc.hostList[i].gameName))
						nc.JoinServer(nc.hostList[i]);
				}
				GUI.EndScrollView();
			}
		}
	}
}
