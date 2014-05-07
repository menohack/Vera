using UnityEngine;
using System.Collections;

public class EquipGUI : MonoBehaviour {

	/// <summary>
	/// The list of textures for each item.
	/// </summary>
	public Texture[] equipment;

	RightHandToolSelector t;

	// Update is called once per frame
	void Update () {
		t = Spawn.GetCurrentPlayer ().GetComponent<RightHandToolSelector> ();
		changeTexture ();
	}

	/// <summary>
	/// Hacky!
	/// </summary>
	void changeTexture()
	{
		switch(t.getCurrentTool())
		{
		case RightHandToolSelector.Tool.Sword:
			this.guiTexture.texture = equipment[0];
			break;
		case RightHandToolSelector.Tool.Axe:
			this.guiTexture.texture = equipment[1];
			break;
		case RightHandToolSelector.Tool.Pickaxe:
			this.guiTexture.texture = equipment[2];
			break;
		default:
			break;
		}
	}
}
