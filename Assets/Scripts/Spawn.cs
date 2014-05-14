using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
	/// <summary>
	/// The terrain is needed to spawn the wolves at the correct height.
	/// </summary>
	public Terrain terrain;

	/// <summary>
	/// The wolf prefab.
	/// </summary>
	public GameObject wolf;

	/// <summary>
	/// Three points that specify the player spawn area.
	/// </summary>
	public Transform playerSpawn1, playerSpawn2, playerSpawn3;

	/// <summary>
	/// The player prefab.
	/// </summary>
	public GameObject playerPrefab;

	/// <summary>
	/// The howl to play when wolves are spawned.
	/// </summary>
	public AudioSource howl;

	/// <summary>
	/// The sundial, for keeping track of time.
	/// </summary>
	public Sundial sundial;

	/// <summary>
	/// The base number of wolves to spawn.
	/// </summary>
	public int wolfStartingCount = 5;

	/// <summary>
	/// The minimum radius that wolves are spawned around the player.
	/// </summary>
	public float minWolfSpawnRadius = 10f;

	/// <summary>
	/// The maximum radis that wolves are spawned around the player.
	/// </summary>
	public float maxWolfSpawnRadius = 15f;

	/// <summary>
	/// Spawn can be referenced statically for getting the current player.
	/// </summary>
	public static Spawn instance;

	/// <summary>
	/// Spawn keeps track of the current player GameObject.
	/// </summary>
	GameObject currentPlayer;

	/// <summary>
	/// The mesh of the water for getting its height.
	/// </summary>
	public MeshFilter waterFilter;

	/// <summary>
	/// The height of the water so we don't spawn wolves underwater.
	/// </summary>
	float waterHeight;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			waterHeight = waterFilter.transform.position.y;
			SpawnPlayer();
		}
		else
		{
			Debug.Log("There should be only one Spawn object in the scene");
			Destroy(this);
		}
	}


	/// <summary>
	/// Gets the current player GameObject.
	/// </summary>
	/// <returns>The current player.</returns>
	public static GameObject GetCurrentPlayer()
	{
		return instance.currentPlayer;
	}


	/// <summary>
	/// Returns a random point within a triangle specified by playerSpawn1-3.
	/// </summary>
	/// <returns>A random spawn point.</returns>
	Vector3 GetRandomPlayerSpawn()
	{
		Vector2 random = new Vector2(Random.value, Random.value);
		random.Normalize();
		return playerSpawn1.position + random.x * (playerSpawn2.position - playerSpawn1.position)
			+ random.y * (playerSpawn3.position - playerSpawn1.position);
	}

	/// <summary>
	/// Spawn your player at the spawn area.
	/// </summary>
	void SpawnPlayer()
	{
		if (Network.connections.Length > 0)
			currentPlayer = Network.Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation, 0) as GameObject;
		else
			currentPlayer = Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation) as GameObject;
	}

	/// <summary>
	/// Respawns the player after a death.
	/// </summary>
	public void RespawnPlayer()
	{
		if (Network.connections.Length > 0)
			currentPlayer = Network.Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation, 0) as GameObject;
		else
			currentPlayer = Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation) as GameObject;
	}

	/// <summary>
	/// Spawns wolves based on the day number around the current player.
	/// </summary>
	public void SpawnWolves()
	{
		if (Network.isServer || !NetworkController.IsMultiplayerGame())
		{
			GameObject player = Spawn.GetCurrentPlayer();
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

	/// <summary>
	/// Removes all wolves from the scene.
	/// </summary>
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
			float x, y, z;
			do
			{
				float angle = Random.Range(0, 360);
				float distance = Random.Range(minRadius, maxRadius);
				z = distance / Mathf.Sin(Mathf.Deg2Rad * angle);
				x = distance / Mathf.Cos(Mathf.Deg2Rad * angle);
				y = terrain.SampleHeight(new Vector3(position.x + x, 0, position.z + z));
			} while (float.IsInfinity(x) || float.IsInfinity(z) || float.IsNaN(x) || float.IsNaN(z) || y < waterHeight);

			Vector3 result = new Vector3(position.x + x, y, position.z + z);

			GameObject spawn = Utility.InstantiateHelper(wolf, result, wolf.transform.rotation);
			spawn.name = "Wolf";
		}
	}
}
