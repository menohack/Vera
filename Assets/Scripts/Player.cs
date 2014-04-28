using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Texture2D healthBarFront;

	public Texture2D healthBarBack;

	public GUIStyle style;

	void Start()
	{
		Screen.showCursor = false;
	}

	void Update()
	{
		if (Network.isServer || Network.connections.Length == 0)
			if (Input.GetKey(KeyCode.Escape) && !Menu.Paused() && !Menu.GameOver())
				Menu.Pause();
	}

	/// <summary>
	/// Remove keyboard and mouse controls locally from other players.
	/// </summary>
	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		ThirdPersonController controller = gameObject.GetComponent<ThirdPersonController>();
		if (controller != null && !networkView.isMine)
		{
			Destroy(gameObject.GetComponent<ThirdPersonController>());
			Destroy(gameObject.GetComponent<MouseLook>());
			Transform t = gameObject.transform.FindChild("CamLookPoint");
			if (t != null)
				Destroy(t.gameObject);
			Destroy(gameObject.GetComponent<Build>());
			MeleeCollider[] mcs = FindObjectsOfType<MeleeCollider>();
			foreach (MeleeCollider m in mcs)
			{
				Transform temp = m.transform;
				while (temp.parent != null)
					temp = temp.parent;
				if (temp.networkView && !temp.networkView.isMine)
					Destroy(m);
			}
			gameObject.rigidbody.isKinematic = true;
			Destroy(this);
		}
	}
}
