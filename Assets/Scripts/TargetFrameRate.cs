using UnityEngine;
using System.Collections;

public class TargetFrameRate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 30;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
