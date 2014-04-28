using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour {

	private const string typeName = "Vera Online";
	private const string gameName = "Vera Game";

	public HostData[] hostList;

	bool connected = false;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public bool Connected()
	{
		return connected;
	}

	public void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
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
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log("Disconnected from server: " + info);
		connected = false;

		//We should delete other player's RPCs and objects here
	}
}
