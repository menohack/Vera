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

	void Awake()
	{
		SpawnPlayer();
	}

	public void SpawnWorld()
	{
		Vector3 terrainSize = terrain.terrainData.size;

		//Spawn the player randomly
		//MovePlayer(terrainSize);

		//Spawn resources randomly
		SpawnTrees(treeCount, terrainSize);
		SpawnOre(oreCount, terrainSize);
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
			Network.Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation, 0);
		else
			Instantiate(playerPrefab, GetRandomPlayerSpawn(), playerPrefab.transform.rotation);
	}

	/// <summary>
	/// Remove keyboard and mouse controls locally from other players.
	/// </summary>
	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		GameObject g = info.networkView.gameObject;
		ThirdPersonController controller =  gameObject.GetComponent<ThirdPersonController>();
		if (controller != null && !info.networkView.isMine)
		{
			Debug.Log("Removing ThirdPersonController and MouseLook scripts from " + info.sender);
			Destroy(g.GetComponent<ThirdPersonController>());
			Destroy(g.GetComponent<MouseLook>());
			Destroy(g.GetComponentInChildren<MouseLook>());
		}
	}

	void SpawnTrees(int count, Vector3 size)
	{
		GameObject[] trees = { tree1, tree2, tree3 };
		SpawnObjects(count, size, trees);
	}

	void SpawnOre(int count, Vector3 size)
	{
		GameObject[] ores = { ore };
		SpawnObjects(count, size, ores);
	}

	void SpawnObjects(int count, Vector3 size, GameObject[] types)
	{
		for (int i = 0; i < count; i++)
		{
			//Type of tree
			GameObject spawnType = types[Mathf.FloorToInt(Random.value * types.Length)];
			GameObject spawn = Instantiate(spawnType) as GameObject;

			//Position
			Vector3 randomPosition = new Vector3(terrain.transform.position.y + Random.value * size.x, 0, terrain.transform.position.z + Random.value * size.z);

			randomPosition.y = terrain.SampleHeight(randomPosition);
			spawn.transform.position = randomPosition;

			//Rotation
			//Trees need to be default unrotated for this to work
			//spawn.transform.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
		}
	}

	public void SpawnWolves()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if (player && sundial) 
		{
			int numWolves = wolfStartingCount + sundial.GetDay() * sundial.GetDay();
			SpawnWolves (numWolves, 10f, 15f, player.transform.position, player.transform);

			if (howl)
				howl.audio.Play();
			else
				Debug.Log("Howl audio source missing");
		}
		else
			Debug.Log("Can't find player or sundial for Spawn script");
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
			GameObject spawn = Instantiate(wolf) as GameObject;
			spawn.name = "Wolf";
			SeekerAI seek = spawn.GetComponent<SeekerAI>();
			seek.target = tgt;

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
			spawn.transform.position = result;
		}
	}
}
