using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] 
public class LoadingScreen : MonoBehaviour {

	public Texture2D titleScreen, infoScreen, controlScreen;

	bool loading = false, controls = false, info = false;

	Texture2D texture;

	void Start()
	{
		texture = titleScreen;
		Screen.showCursor = true;
		Screen.lockCursor = false;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	[RPC]
	public void StartGame()
	{
		loading = true;
		Application.LoadLevel("Level1");
	}

	void OnGUI()
	{

		//GUI.Box(new Rect(0, 0, loadingScreen.width, loadingScreen.height), loadingScreen, style);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture, ScaleMode.ScaleAndCrop);
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 32;
		labelStyle.alignment = TextAnchor.UpperCenter;

		if (loading)
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height * 0.9f, 200, 100), "LOADING", labelStyle);

		NetworkController nc = GetComponent<NetworkController>();

		if (!loading)
		{
			if ((!nc.Connected() || Network.isServer) && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height * 0.9f, 100, 40), "Start Game"))
			{
				if (Network.isServer)
					networkView.RPC("StartGame", RPCMode.AllBuffered);
				else
					StartGame();
			}
			if (!controls && GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height * 0.9f, 100, 40), "Controls"))
			{
				controls = true;
				info = false;
				texture = controlScreen;
			}
			if (!info && GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height * 0.9f, 100, 40), "Info"))
			{
				info = true;
				controls = false;
				texture = infoScreen;
			}
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height * 0.9f + 40, 100, 40), "Quit"))
				Application.Quit();
		}

		

		if (nc != null && !nc.Connected() && !Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				nc.StartServer();

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				nc.RefreshHostList();

			if (nc.hostList != null)
			{
				GUI.BeginScrollView(new Rect(400, 100, 300, 400), Vector2.zero, new Rect(0, 0, 300, 300));
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
