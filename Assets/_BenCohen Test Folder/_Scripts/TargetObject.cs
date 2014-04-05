using UnityEngine;
using System.Collections;

public class TargetObject : MonoBehaviour
{
		public NavMeshAgent agent;

		void Start ()
		{
				agent.destination = transform.position;
		}

		void Update ()
		{
				agent.destination = transform.position;
		}
}

