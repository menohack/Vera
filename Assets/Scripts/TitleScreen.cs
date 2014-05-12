using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode()] 
public class TitleScreen : MonoBehaviour {

	public Texture2D titleScreen, singleplayerScreen, multiplayerScreen, infoScreen, controlScreen;
	public Texture2D arrowLeft, arrowRight, multiplayerTexture, singleplayerTexture, quitTexture;

	bool loading = false, controls = false, info = false;

	Texture2D texture;

	NetworkController nc;

	public Font font;

	string ipFieldString = "Server IP";

	public Texture2D cursorTexture;

	/// <summary>
	/// The position from which to compute the mouse offset.
	/// </summary>
	Vector3 mouseStart;

	/// <summary>
	/// The state of the title screen. Left is singleplayer and right is multiplayer.
	/// </summary>
	enum ScreenState
	{
		Singleplayer,
		ScrollLeft,
		Title,
		ScrollRight,
		Multiplayer
	}

	/// <summary>
	/// The current state of the title screen.
	/// </summary>
	ScreenState state = ScreenState.Title;

	/// <summary>
	/// The previous state of the title screen, used to scroll properly.
	/// </summary>
	ScreenState previousState = ScreenState.Title;

	/// <summary>
	/// The time that scrolling started.
	/// </summary>
	DateTime scrollStart;

	/// <summary>
	/// The speed that the screen moves onces swiped.
	/// </summary>
	public float swipeSpeed = 1000f;

	/// <summary>
	/// The offset of the title screen in pixels.
	/// </summary>
	float screenOffset = 0f;

	/// <summary>
	/// Whether the mouse needs to be moved back to the center of the screen to prevent
	/// moving past the title screen when swiping back to it.
	/// </summary>
	bool swipeReset = false;

	/// <summary>
	/// The mouse position ratio at which to transition to another screen.
	/// </summary>
	const float SCREEN_TRANSITION_RATIO = 0.5f;

	/// <summary>
	/// The mouse position ratio that resets the swipe mechanism.
	/// </summary>
	const float SCREEN_RESET_RATIO = 0.1f;

	void Start()
	{
		texture = titleScreen;
		
		Screen.showCursor = true;
		Screen.lockCursor = false;

		nc = FindObjectOfType<NetworkController>();

		mouseStart = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
	}

	void Update()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	[RPC]
	public void StartGame()
	{
		loading = true;
		Application.LoadLevel("Level1");
	}

	/// <summary>
	/// A cubic spline.
	/// </summary>
	/// <param name="t">The parameter between [-1,1].</param>
	/// <returns>The spline evaluated at t.</returns>
	float Spline(float t)
	{
		return t * t * t;
	}

	/// <summary>
	/// Computes the offset of the title screen based on the position of the mouse. The design
	/// of the title screen is simple enough not to warrant a full class implementing the State pattern.
	/// </summary>
	/// <param name="t">The mouse screen position between [-1,1].</param>
	/// <returns>The offset in pixels of the title screen.</returns>
	float ComputeScreenOffset(float t)
	{
		if (state == ScreenState.Title)
		{
			if (swipeReset)
			{
				if (t < SCREEN_RESET_RATIO && t > -SCREEN_RESET_RATIO)
					swipeReset = false;
				else
					return 0f;
			}
			
			previousState = ScreenState.Title;	
			if (t > SCREEN_TRANSITION_RATIO && !swipeReset)
			{
				state = ScreenState.ScrollLeft;
				scrollStart = DateTime.Now;
			}
			else if (t < -SCREEN_TRANSITION_RATIO && !swipeReset)
			{
				state = ScreenState.ScrollRight;
				scrollStart = DateTime.Now;
			}
			return Screen.width * Mathf.Clamp(Spline(t), -1.0f, 1.0f);
		}
		else if (state == ScreenState.Singleplayer)
		{
			if (t < -SCREEN_TRANSITION_RATIO)
			{
				state = ScreenState.ScrollRight;
				previousState = ScreenState.Singleplayer;
				scrollStart = DateTime.Now;
			}
			return Screen.width * Mathf.Clamp(Spline(t), -1.0f, 0f) + Screen.width;
		}
		else if (state == ScreenState.ScrollRight)
		{
			float swipeOffset = (float)((DateTime.Now - scrollStart).TotalSeconds) * -swipeSpeed;
			if (swipeOffset + screenOffset < 0 && previousState == ScreenState.Singleplayer)
			{
				swipeReset = true;
				state = ScreenState.Title;
				return 0f;
			}
			else if (swipeOffset + screenOffset < -Screen.width && previousState == ScreenState.Title)
			{
				state = ScreenState.Multiplayer;
				return -Screen.width;
			}
			else
				return swipeOffset + screenOffset;
		}
		else if (state == ScreenState.ScrollLeft)
		{
			float swipeOffset = (float)((DateTime.Now - scrollStart).TotalSeconds) * swipeSpeed;
			if (swipeOffset + screenOffset > 0f && previousState == ScreenState.Multiplayer)
			{
				swipeReset = true;
				state = ScreenState.Title;
				return 0f;
			}
			else if (swipeOffset + screenOffset > Screen.width && previousState == ScreenState.Title)
			{
				state = ScreenState.Singleplayer;
				return Screen.width;
			}
			else
				return swipeOffset + screenOffset;
			 
		}
		else if (state == ScreenState.Multiplayer)
		{
			if (t > SCREEN_TRANSITION_RATIO)
			{
				state = ScreenState.ScrollLeft;
				previousState = ScreenState.Multiplayer;
				scrollStart = DateTime.Now;
			}
			return Screen.width * Mathf.Clamp(Spline(t), 0f, 1.0f) - Screen.width;
		}
		else
			throw new UnityException("Invalid screen state");
	}

	void OnGUI()
	{
		float mouseDelta = mouseStart.x - Input.mousePosition.x;
		float t = mouseDelta / (Screen.width / 2.0f);

		if (!loading)
			screenOffset = ComputeScreenOffset(t);

		//Screens
		GUI.DrawTexture(new Rect(screenOffset, 0, Screen.width, Screen.height), texture, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture(new Rect(screenOffset - Screen.width, 0, Screen.width, Screen.height), singleplayerScreen, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture(new Rect(screenOffset + Screen.width, 0, Screen.width, Screen.height), multiplayerScreen, ScaleMode.ScaleAndCrop);

		//Arrows
		GUI.DrawTexture(new Rect(screenOffset, Screen.height / 2 + singleplayerTexture.height, arrowLeft.width, arrowLeft.height), arrowLeft);
		GUI.DrawTexture(new Rect(screenOffset + Screen.width - arrowRight.width, Screen.height / 2 + multiplayerTexture.height, arrowRight.width, arrowRight.height), arrowRight);

		//Singleplayer/multiplayer labels
		GUI.DrawTexture(new Rect(screenOffset, Screen.height / 2, singleplayerTexture.width, singleplayerTexture.height), singleplayerTexture);
		GUI.DrawTexture(new Rect(screenOffset + Screen.width - multiplayerTexture.width, Screen.height / 2, multiplayerTexture.width, multiplayerTexture.height), multiplayerTexture);

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.font = font;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 32;
		labelStyle.alignment = TextAnchor.UpperCenter;

		if (loading)
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height * 0.9f, 200, 100), "LOADING", labelStyle);

		if (!loading)
		{
			if (Network.peerType == NetworkPeerType.Server && GUI.Button(new Rect(screenOffset + Screen.width + Screen.width / 2 - 50, Screen.height * 0.9f, 100, 40), "Start Game", labelStyle))
				networkView.RPC("StartGame", RPCMode.AllBuffered);
			if (Network.peerType == NetworkPeerType.Disconnected && GUI.Button(new Rect(screenOffset + -Screen.width + Screen.width / 2 - 50, Screen.height * 0.9f, 100, 40), "Start Game", labelStyle))
				StartGame();
			
			if (GUI.Button(new Rect(screenOffset + Screen.width / 2 - quitTexture.width/2, Screen.height - quitTexture.height, quitTexture.width, quitTexture.height), quitTexture, labelStyle))
				Application.Quit();
		}



		if (nc != null && !nc.Connected() && !Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(screenOffset + Screen.width + Screen.width/2, 100, 250, 100), "Start Server"))
				nc.StartServer();

			if (GUI.Button(new Rect(screenOffset + Screen.width + Screen.width / 2, 250, 250, 100), "Refresh Hosts"))
				nc.RefreshHostList();

			ipFieldString = GUI.TextField(new Rect(screenOffset + Screen.width + Screen.width / 2, 400, 250, 50), ipFieldString);
			if (GUI.Button(new Rect(screenOffset + Screen.width + Screen.width / 2 + 250, 400, 80, 50), "Connect"))
 				nc.JoinServer(ipFieldString);

			if (nc.hostList != null)
			{
				GUI.BeginScrollView(new Rect(screenOffset + Screen.width + Screen.width/2 + 400, 100, 300, 400), Vector2.zero, new Rect(0, 0, 300, 300));
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
