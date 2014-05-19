using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour {

	private const string typeName = "Vera Online";

	public static int DEFAULT_PORT = 25000;

	public HostData[] hostList;

	static bool multiplayer = false;

	bool connected = false;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	/// <summary>
	/// Returns whether we are playing a multiplayer game.
	/// </summary>
	/// <returns>True if we are playing multiplayer.</returns>
	public static bool IsMultiplayerGame()
	{
		return multiplayer;
	}

	public bool Connected()
	{
		return connected;
	}

	public void StartServer(string gameName)
	{
		Network.InitializeServer(32, DEFAULT_PORT, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		multiplayer = true;
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	public void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	public void JoinServer(HostData hostData)
	{
		Debug.Log("Attempting to connect to server");
		Network.Connect(hostData);
	}

	public void JoinServer(string ip)
	{
		Debug.Log("Attempting to connect to server");
		Network.Connect(ip, DEFAULT_PORT);
	}

	int playerCount = 0;

	void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);
		connected = true;
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		if (playerCount == 0)
		{
			connected = false;
			Network.Disconnect();
		}
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");
		connected = true;
		multiplayer = true;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log("Disconnected from server: " + info);
		connected = false;

		//We should delete other player's RPCs and objects here
	}

	/// <summary>
	/// Enable the message queue once a level is loaded.
	/// </summary>
	/// <param name="level">The level.</param>
	void OnLevelWasLoaded(int level)
	{
		Network.SetSendingEnabled(0, true);
		Network.isMessageQueueRunning = true;
	}
}
