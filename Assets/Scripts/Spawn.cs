using UnityEngine;
using System.Collections;
using Pathfinding;

public class Spawn : MonoBehaviour {

	public Terrain terrain;
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;
	public GameObject ore;
	public GameObject wolf;

	public int treeCount = 1000;
	public int oreCount = 1000;

	void Start()
	{
		SpawnWorld();
	}

	public void SpawnWorld()
	{
		Vector3 terrainSize = terrain.terrainData.size;

		//Spawn the player randomly
		//MovePlayer(terrainSize);

		//Spawn resources randomly
		SpawnTrees(treeCount, terrainSize);
		SpawnOre(oreCount, terrainSize);
		GameObject player = GameObject.FindWithTag("Player");
		SpawnWolves(100, 20f, 50f, player.transform.position, player.transform);
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

	void MovePlayer(Vector3 size)
	{
	}

	/// <summary>
	/// Spawns count wolves centered around position within minRadius and maxRadius radial distance.
	/// </summary>
	/// <param name="count">The number of wolves.</param>
	/// <param name="minRadius">The minimum radius.</param>
	/// <param name="maxRadius">The maximum radius</param>
	/// <param name="position">The position about which to spawn the wolves.</param>
	public void SpawnWolves(int count, float minRadius, float maxRadius, Vector3 position, Transform target)
	{
		if (minRadius < 0f || minRadius > maxRadius || count < 0)
			throw new UnityException("Invalid parameters to SpawnWolves");

		for (int i = 0; i < count; i++)
		{
			GameObject spawn = Instantiate(wolf) as GameObject;
			float angle = Random.Range(0, 360);
			float distance = Random.Range(minRadius, maxRadius);
			float z = distance / Mathf.Sin(Mathf.Deg2Rad * angle);
			float x = distance / Mathf.Cos(Mathf.Deg2Rad* angle);
			spawn.transform.position = position + new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
			//Point the 'seeker' object on the spawned wolves to target (in this case, the player Transform)
			SeekerAI seeker = spawn.GetComponent<SeekerAI> ();
			if (seeker != null) seeker.target = target;
		}
	}
}
