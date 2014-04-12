/*
Simple level of detail script.
It allows for up to 2 lower level of detail.
If nothing is assigned to lod0, this game object will be assigned to lod0.
It is ok to only have 1 lower level og detail, lod2 will simply be ignored.
If Cull Mesh is checked the mesh will be culled when max distance is reached.
The lod changes at intervals of a 1/3 of max distance.
*/

#pragma strict
@script AddComponentMenu ("Mesh/LOD")

private var player : GameObject;

var lod0 : GameObject;
var lod1 : GameObject;
var lod2 : GameObject;
var cullMesh : boolean = false;
var spreadOverSeconds : float = 5;
var maxDistance : float = 100;

private var distance : float;
private var onGoingCheck : boolean = false;
private var begin : boolean = false;

function Start(){

//find the player for a distance reference point
player = GameObject.FindWithTag ("Player");

//better safe than sorry :P
if (!lod0) lod0 = gameObject;
if (lod0.renderer == null) lod0.AddComponent ("MeshRenderer");

if (lod1 && lod1.renderer == null) lod1.AddComponent ("MeshRenderer");
if (lod1) lod1.renderer.enabled = false;

if (lod2 && lod2.renderer == null) lod2.AddComponent ("MeshRenderer");
if (lod2) lod2.renderer.enabled = false;

//wait random amount of time to spread calculations out over many frames
yield WaitForSeconds (Random.Range(0, spreadOverSeconds));
begin = true;

}

function FixedUpdate () {

	if (player && onGoingCheck == false && begin == true) {
		onGoingCheck = true;
		DistanceCheck();
	}

}

function DistanceCheck(){
	if(lod1){
		distance = Vector3.Distance(player.transform.position, transform.position);

		if (distance < maxDistance/3){
			lod0.renderer.enabled = true;
			lod1.renderer.enabled = false;
			if(lod2) lod2.renderer.enabled = false;
		}

		if (distance > maxDistance/3){
			lod0.renderer.enabled = false;
			lod1.renderer.enabled = true;
			if(lod2) lod2.renderer.enabled = false;
		}

		if (distance > maxDistance/3*2 && lod2){
			lod0.renderer.enabled = false;
			lod1.renderer.enabled = false;
			lod2.renderer.enabled = true;
		}

		if (distance > maxDistance && cullMesh == true){
			lod0.renderer.enabled = false;
			lod1.renderer.enabled = false;
			if(lod2) lod2.renderer.enabled = false;
		}

		Debug.Log ("LodTestTime: " + Time.time + "distance: " + distance);

		yield WaitForSeconds (spreadOverSeconds);
	}
	onGoingCheck = false;
}

// Coded by Lasse Westmark
// Free to use by anyone and anything, and for anything
// credit me is you like, don't if you don't ;P