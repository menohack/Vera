using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	public Terrain terrainPrefab;

	public Terrain oreTile;
	public Terrain treesTile;
	public Terrain waterTile;
	public Terrain landTile;

	/// <summary>
	/// The size of the terrain block in game units.
	/// </summary>
	float blockSize = 1000.0f;

	/// <summary>
	/// The maximum number of terrain blocks.
	/// </summary>
	const int NUM_MAX_BLOCKS = 16;

	
	void Start ()
	{
		Networking.Instance.GetMap();
		/* Random terrain
		SeedGenerator();

		for (int i = -1; i < 2; i++)
			for (int j = -1; j < 2; j++)
				GenerateTerrainBlock(i, j);
		 */
	}

	public class Tuple<T1, T2>
	{
		public T1 First { get; private set; }
		public T2 Second { get; private set; }
		internal Tuple(T1 first, T2 second)
		{
			First = first;
			Second = second;
		}
	}

	public static class Tuple
	{
		public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
		{
			var tuple = new Tuple<T1, T2>(first, second);
			return tuple;
		}
	}

	public enum TerrainType
	{
		Ore, Trees, Water, Fertile
	}

	[DataContract(Name = "Ballz")]
	public class GameData
	{
		[DataMember(Name = "Derp")]
		public Dictionary<TerrainType, Tuple<int, int>> terrain = new Dictionary<TerrainType, Tuple<int, int>>();
	}

	/// <summary>
	/// Seed Simplex noise generator randomly.
	/// </summary>
	void SeedGenerator()
	{
		byte[] noiseSeed = new byte[512];
		System.Random rand = new System.Random();
		rand.NextBytes(noiseSeed);
		SimplexNoise.Noise.perm = noiseSeed;
	}

	/// <summary>
	/// Generates a heightmap using Simplex Noise. Each block has an x and y index.
	/// </summary>
	/// <param name="xIndex">The x-index of the block.</param>
	/// <param name="yIndex">The y-index of the block.</param>
	/// <returns></returns>
	Terrain GenerateTerrainBlock(int xIndex, int yIndex)
	{
		int xShift = xIndex + NUM_MAX_BLOCKS / 2;
		int yShift = yIndex + NUM_MAX_BLOCKS / 2;
		if (xShift < 0 || yShift < 0)
			throw new UnityException("Error: Terrain indices must be positive");
		if (xShift > NUM_MAX_BLOCKS || yShift > NUM_MAX_BLOCKS)
			throw new UnityException("Error: Too many terrain blocks");
		int resolution = terrainPrefab.terrainData.heightmapResolution;
		float[,] heights = new float[resolution, resolution];

		for (int i = 0; i < 10; i++)
		{
			float f = Mathf.Pow(2.0f, i);
			PerlinIteration(heights, 1 / (100.0f / f), 1 / (100.0f / f), xShift, yShift, 1.0f / f);
		}

		Terrain terrain = Instantiate(terrainPrefab) as Terrain;
		terrain.terrainData = Instantiate(terrainPrefab.terrainData) as TerrainData;
		//terrain.terrainData.heightmapResolution = resolution;
		terrain.terrainData.SetHeights(0, 0, heights);
		terrain.transform.position += new Vector3(yIndex * blockSize, 0.0f, xIndex * blockSize);
		//terrain.gameObject.AddComponent<TerrainCollider>();
		TerrainCollider collider = terrain.gameObject.collider as TerrainCollider;
		collider.terrainData = terrain.terrainData;
		

		PlaceTrees(terrain, 1000);

		return terrain;
	}

	void PerlinIteration(float[,] heights, float xScale, float yScale, int xShift, int yShift, float intensity)
	{
		for (int i = 0; i < heights.GetLength(0); i++)
		{
			for (int j = 0; j < heights.GetLength(1); j++)
			{
				int x = xShift * heights.GetLength(0) + i;
				int y = yShift * heights.GetLength(1) + j;
				heights[i, j] += intensity * 0.1f * (SimplexNoise.Noise.Generate(x * xScale, y * yScale) + 0.5f);
			}
		}
	}

	/// <summary>
	/// Places count trees randomly.
	/// </summary>
	/// <param name="terrain">The terrain on which to place trees.</param>
	/// <param name="count">The number of trees.</param>
	static void PlaceTrees(Terrain terrain, int count)
	{
		TreeInstance tree = new TreeInstance();
		
		tree.heightScale = 1.0f;
		tree.widthScale = 1.0f;
		tree.prototypeIndex = 0;
		tree.color = Color.white;
		tree.lightmapColor = Color.white;

		for (int i = 0; i < count; i++)
		{
			tree.position = new Vector3(UnityEngine.Random.value, 0.0f, UnityEngine.Random.value);
			terrain.AddTreeInstance(tree);
		}
	}
}
