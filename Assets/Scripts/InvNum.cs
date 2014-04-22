using UnityEngine;
using System.Collections;

public class InvNum : MonoBehaviour {

	/// <summary>
	/// Wood or Ore?
	/// </summary>
	public bool isWood = true;

	private GameObject player;
	private Inventory inv;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		inv = player.gameObject.GetComponent<Inventory> ();
		if (isWood) 
		{
			guiText.text = "" + inv.GetWood ();
		}
		else
		{
			guiText.text = "" + inv.GetOre ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isWood) 
		{
			guiText.text = "" + inv.GetWood ();
		}
		else
		{
			guiText.text = "" + inv.GetOre ();
		}
	}
}
