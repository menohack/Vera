using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SeekerAI))]
[RequireComponent(typeof(NavMeshAgent))]
public class NavSelector : MonoBehaviour {

	protected SeekerAI seekerAI;
	
	protected NavMeshAgent navMeshAgent;

	protected Bounds bounds;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
