using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public Terrain terrain;
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;
	public GameObject ore;
	public GameObject wolf;

	public Transform playerSpawn1, playerSpawn2, playerSpawn3;

	public GameObject playerPrefab;

	public int treeCount = 1000;
	public int oreCount = 1000;

	public AudioSource howl;

	public Sundial sundial;

	public int wolfStartingCount = 5;

	public float minWolfSpawnRadius = 10f;
	public float maxWolfSpawnRadius = 15f;

	public static Spawn instance;

	GameObject myPlayer;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			SpawnPlayer();
		}
		else
		{
			Debug.Log("There should be only one Spawn object in the scene");
			Destroy(this);
		}
	}

	public static GameObject GetMyPlayer()
	{
		return instance.myPlayer;
	}


	Vector3 GetRandomPlayerSpawn()
	{
		Vector2 random = new Vector2(Random.value, Random.value);
		random.Normalize();
		return playerSpawn1.position + random.x * (playerSpawn2.position - playerSpawn1.position)
			+ random.y * (playerSpawn3.position - playerSpawn1.position);
	}

	/// <summary>
	/// Spawn your player.
	/// </summary>
	void SpawnPlayer()
	{
		if (Network.connections.Length > 0)
			myPlayer = Network.Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation, 0) as GameObject;
		else
			myPlayer = Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation) as GameObject;
	}

	/// <summary>
	/// Respawns the player after a death.
	/// </summary>
	public void RespawnPlayer()
	{
		GameObject player;
		if (Network.connections.Length > 0)
			player = Network.Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation, 0) as GameObject;
		else
			player = Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation) as GameObject;
	}

	public void SpawnWolves()
	{
		if (Network.isServer || Network.connections.Length == 0)
		{
			GameObject player = GameObject.FindWithTag("Player");
			if (player && sundial)
			{
				int numWolves = wolfStartingCount + sundial.GetDay() * sundial.GetDay();
				SpawnWolves(numWolves, minWolfSpawnRadius, maxWolfSpawnRadius, player.transform.position, player.transform);

				if (howl)
					howl.audio.Play();
				else
					Debug.Log("Howl audio source missing");
			}
			else
				Debug.Log("Can't find player or sundial for Spawn script");
		}
	}

	public void DespawnWolves()
	{
		GameObject[] wolves = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject w in wolves)
			Destroy(w);
	}

	/// <summary>
	/// Spawns count wolves centered around position within minRadius and maxRadius radial distance.
	/// </summary>
	/// <param name="count">The number of wolves.</param>
	/// <param name="minRadius">The minimum radius.</param>
	/// <param name="maxRadius">The maximum radius</param>
	/// <param name="position">The position about which to spawn the wolves.</param>
	public void SpawnWolves(int count, float minRadius, float maxRadius, Vector3 position, Transform tgt)
	{
		if (minRadius < 0f || minRadius > maxRadius || count < 0)
			throw new UnityException("Invalid parameters to SpawnWolves");

		for (int i = 0; i < count; i++)
		{
			float x, z;
			do
			{
				float angle = Random.Range(0, 360);
				float distance = Random.Range(minRadius, maxRadius);
				z = distance / Mathf.Sin(Mathf.Deg2Rad * angle);
				x = distance / Mathf.Cos(Mathf.Deg2Rad * angle);
			} while (float.IsInfinity(x) || float.IsInfinity(z) || float.IsNaN(x) || float.IsNaN(z));

			//added 8 to Y to avoid spawning under terrain. unsure why terrain.SampleHeight isn't working on the map
			Vector3 result = new Vector3(position.x + x, terrain.SampleHeight(new Vector3(position.x + x, 0, position.z + z)), position.z + z);

			GameObject spawn;
			if (Network.connections.Length > 0)
				spawn = Network.Instantiate(wolf, result, wolf.transform.rotation, 0) as GameObject;
			else
				spawn = Instantiate(wolf, result, wolf.transform.rotation) as GameObject;
			spawn.name = "Wolf";
		}
	}
}
