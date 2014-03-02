using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Text;

public class Networking
{
	static Networking instance;

	private Networking()
	{
	}

	public static Networking Instance
	{
		get
		{
			if (instance == null)
				instance = new Networking();
			return instance;
		}
		private set {}
	}

	TcpClient client;
	static int READ_BUFFER_SIZE = 256;
	static int WRITE_BUFFER_SIZE = 256;
	byte[] readBuffer = new byte[READ_BUFFER_SIZE];
	byte[] writeBuffer = new byte[WRITE_BUFFER_SIZE];

	public bool GetMap()
	{
		//Debug.Log("Result from server: " + web.DownloadString("http://127.0.0.1:11000"));
		client = new TcpClient("127.0.0.1",11000);
		NetworkStream stream = client.GetStream();

		byte[] message = Encoding.ASCII.GetBytes("Map plz\n");
		Buffer.BlockCopy(message, 0, writeBuffer, 0, message.Length);
		stream.BeginWrite(writeBuffer, 0, message.Length, new AsyncCallback(Write), null);
		return true;
	}

	void Write(IAsyncResult result)
	{
		client.GetStream().EndWrite(result);
		Debug.Log("Finished writing");

		client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(Read), null);
	}

	void Read(IAsyncResult result)
	{
		int bytesRead = client.GetStream().EndRead(result);
		Debug.Log(bytesRead + " bytes read");
		if (bytesRead < 1)
			return;
		string message = Encoding.ASCII.GetString(readBuffer, 0, bytesRead);
		Debug.Log("Message: " + message);

		client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(Read), null);
	}

}
