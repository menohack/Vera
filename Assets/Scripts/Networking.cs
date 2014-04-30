using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using VeraLibrary;

public class Networking : MonoBehaviour
{
	
	DateTime? serverStartTime = null;
	TimeSpan serverWaitBeforeConnectionTime;
	public float serverWaitBeforeConnectionTimeMillis = 5000f;

	void Start()
	{
		serverWaitBeforeConnectionTime = TimeSpan.FromMilliseconds(serverWaitBeforeConnectionTimeMillis);

		//NetworkConnectionError initializeError = Network.InitializeServer(32, 1337, !Network.HavePublicAddress());
		//MasterServer.RegisterHost("Vera Online", "Game");
		clientWaitForServerTime = TimeSpan.FromMilliseconds(clientWaitForServerTimeMillis);
		hostPollStartTime = DateTime.Now;
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("Vera Game");
	}

	DateTime? hostPollStartTime;
	TimeSpan clientWaitForServerTime;
	public float clientWaitForServerTimeMillis = 5000f;

	void StartMasterServer()
	{
		serverStartTime = DateTime.Now;
		NetworkConnectionError initializeError = Network.InitializeServer(32, 1337, !Network.HavePublicAddress());
		MasterServer.RegisterHost("Vera Online", "Game");
	}

	void Update()
	{
		if (hostPollStartTime != null && DateTime.Now - hostPollStartTime > clientWaitForServerTime)
		{
			
		}
		else if (MasterServer.PollHostList().Length != 0)
		{
			HostData[] hostData = MasterServer.PollHostList();
			int i = 0;
			while (i < hostData.Length)
			{
				Debug.Log("Game name: " + hostData[i].gameName);
				i++;
			}
			MasterServer.ClearHostList();
		}
	}
	 

	void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.Log("Failed to connect to server: " + error);
		/*
		Debug.Log("Failed to connect to server, attempting to host server...");
		bool useNAT = !Network.HavePublicAddress();
		NetworkConnectionError initializeError = Network.InitializeServer(32, 1337, useNAT);
		if (initializeError != NetworkConnectionError.NoError)
			Connect();
		*/
	}

	
	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");
	}

	int playerCount = 0;

	void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		playerCount--;
		//Network.RemoveRPCs(player);
		//Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isServer)
			Debug.Log("Local server connection disconnected: " + info);
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
			else
				Debug.Log("Successfully diconnected from the server");
	}

	void OnServerInitialized()
	{
		Debug.Log("Server initialized and ready");
		serverStartTime = DateTime.Now;
	}

	/*
	void Update()
	{
		
		if (playerCount == 0 && serverStartTime != null && ((DateTime.Now - serverStartTime) > serverWaitBeforeConnectionTime))
		{
			serverStartTime = null;
			Network.Disconnect();
			Debug.Log("Server timed out, attempting to find a server as a client...");
			Connect();
		}
		
	}
*/

	void Connect()
	{
		NetworkConnectionError error = Network.Connect("127.0.0.1", 1337);
		if (error != NetworkConnectionError.NoError)
			throw new UnityException(error.ToString());
	}
}
