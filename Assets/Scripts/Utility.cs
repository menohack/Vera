using UnityEngine;
using System.Collections;

public class Utility
{
	public static GameObject InstantiateHelper(GameObject gameObject)
	{
		return InstantiateHelper(gameObject, gameObject.transform.position, gameObject.transform.rotation);
	}

	public static GameObject InstantiateHelper(GameObject gameObject, Vector3 position, Quaternion rotation)
	{
		if (Network.peerType == NetworkPeerType.Client || Network.peerType == NetworkPeerType.Server)
			return Network.Instantiate(gameObject, position, rotation, 0) as GameObject;
		else
			return GameObject.Instantiate(gameObject, position, rotation) as GameObject;
	}

	/// <summary>
	/// Destroys the GameObject on the network if we are connected and locally if we are not.
	/// </summary>
	/// <param name="gameObject">The GameObject to destroy.</param>
	public static void DestroyHelper(GameObject gameObject)
	{
		if (Network.peerType == NetworkPeerType.Client || Network.peerType == NetworkPeerType.Server)
			Network.Destroy(gameObject);
		else
			GameObject.Destroy(gameObject);
	}
}
