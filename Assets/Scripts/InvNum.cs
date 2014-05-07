using UnityEngine;
using System.Collections;

public class InvNum : MonoBehaviour {

	/// <summary>
	/// Wood or Ore?
	/// </summary>
	public bool isWood = true;

	// Update is called once per frame
	void Update () {

		GameObject player = Spawn.GetCurrentPlayer();
		Inventory inventory = null;
		if (player != null)
			inventory = player.GetComponent<Inventory>();

		if (inventory != null)
		{
			if (isWood)
				guiText.text = "" + inventory.GetWood();
			else
				guiText.text = "" + inventory.GetOre();
		}
	}
}
