using UnityEngine;
using System.Collections;

public class Utility
{
	/// <summary>
	/// Destroys the GameObject on the network if we are connected and locally if we are not.
	/// </summary>
	/// <param name="gameObject">The GameObject to destroy.</param>
	public static void DestroyHelper(GameObject gameObject)
	{
		if (Network.connections.Length > 0)
			Network.Destroy(gameObject);
		else
			GameObject.Destroy(gameObject);
	}
}
