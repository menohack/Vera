using UnityEngine;
using System.Net;
using System;

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

	public static bool GetMap()
	{

		using (WebClient web = new WebClient())
		{
			Debug.Log("Result from server: " + web.DownloadString("http://127.0.0.1:11000"));
			return true;
		}
	}
}
