using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	static bool gameOver = false;

	static bool paused = false;

	public Texture2D menuTexture;
	public Texture2D quitButton;
	public Texture2D resumeButton;


	GUIStyle labelStyle, verticalStyle;

	void Start()
	{
		labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.UpperCenter;
		labelStyle.fontSize = 32;
		labelStyle.fontStyle = FontStyle.Bold;

		verticalStyle = new GUIStyle();
		verticalStyle.padding.left = verticalStyle.padding.right = 10;
	}

	/// <summary>
	/// Serializes and deserializes the Menu script across the network.
	/// </summary>
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		
	}

	public static bool GameOver()
	{
		return gameOver;
	}

	public static void EndGame()
	{
		if (!gameOver)
		{
			Time.timeScale = 0f;
			gameOver = true;

			//set mouse to visible and free
			Screen.lockCursor = false;
			Screen.showCursor = true;

			GameObject musicWorld = GameObject.Find("MusicWorld");
			if (musicWorld && musicWorld.audio)
				musicWorld.audio.Stop();
			GameObject musicMenu = GameObject.Find("MusicMenu");
			if (musicMenu && musicMenu.audio)
			{
				musicMenu.audio.time = 16.6f;
				musicMenu.audio.Play();
			}
			MouseLook[] mouseLook = GameObject.FindObjectsOfType<MouseLook>();
			foreach (MouseLook m in mouseLook)
				Destroy(m);
		}
	}

	public static bool Paused()
	{
		return paused;
	}

	[RPC]
	public static void Pause()
	{
		if (!paused)
		{
			paused = true;
			Time.timeScale = 0f;

			Screen.lockCursor = false;
			Screen.showCursor = true;

			GameObject musicWorld = GameObject.Find("MusicWorld");
			if (musicWorld && musicWorld.audio)
				musicWorld.audio.Pause();
			GameObject musicMenu = GameObject.Find("MusicMenu");
			if (musicMenu && musicMenu.audio)
			{
				musicMenu.audio.time = 16.6f;
				musicMenu.audio.Play();
			}
		}

	}

	[RPC]
	public static void Resume()
	{
		if (paused)
		{
			paused = false;
			Time.timeScale = 1f;

			Screen.lockCursor = true;
			Screen.showCursor = false;

			GameObject musicWorld = GameObject.Find("MusicWorld");
			if (musicWorld && musicWorld.audio)
				musicWorld.audio.Play();
			GameObject musicMenu = GameObject.Find("MusicMenu");
			if (musicMenu && musicMenu.audio)
				musicMenu.audio.Pause();
		}
	}

	public static void StartGame()
	{
		Time.timeScale = 1f;
		gameOver = false;

		//set mouse to visible and free
		Screen.lockCursor = true;
		Screen.showCursor = false;

		Application.LoadLevel(0);
	}

	void OnGUI()
	{
		if (gameOver)
		{
			int width = menuTexture.width;
			int height = menuTexture.height;
			GUIStyle buttonStyle = new GUIStyle();
			GUILayout.BeginArea(new Rect(Screen.width / 2 - width/2, Screen.height / 2 - height/2, width, height));
			GUILayout.BeginArea(new Rect(0,0, menuTexture.width, menuTexture.height), menuTexture);
			GUILayout.EndArea();
			GUILayout.BeginVertical(verticalStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("GAME OVER", labelStyle);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("New Game"))
				StartGame();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(quitButton,buttonStyle,GUILayout.MaxWidth(quitButton.width)))
				Application.Quit();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		else if (paused)
		{
			int width = menuTexture.width;
			int height = menuTexture.height;
			GUIStyle buttonStyle = new GUIStyle();
			GUILayout.BeginArea(new Rect(Screen.width / 2 - width/2, Screen.height / 2 - height/2, width, height));
			GUILayout.BeginArea(new Rect(0, 0, menuTexture.width, menuTexture.height), menuTexture);
			GUILayout.EndArea();
			GUILayout.BeginVertical(verticalStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("PAUSED", labelStyle);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if ((Network.isServer || Network.connections.Length == 0) && GUILayout.Button(resumeButton, buttonStyle, GUILayout.MaxWidth(resumeButton.width)))
			{
				if (Network.isServer)
					networkView.RPC("Resume", RPCMode.AllBuffered);
				else if (Network.connections.Length == 0)
					Resume();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("New Game", buttonStyle))
				StartGame();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(quitButton, buttonStyle, GUILayout.MaxWidth(quitButton.width)))
				Application.Quit();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
