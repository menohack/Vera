using UnityEngine;
using System.Collections;


public class TerrainGenerator : MonoBehaviour {

	public Terrain terrainPrefab;

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
		SeedGenerator();

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				GenerateTerrainBlock(i, j);
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
		if (xIndex < 0 || yIndex < 0)
			throw new UnityException("Error: Terrain indices must be positive");
		if (xIndex > NUM_MAX_BLOCKS || yIndex > NUM_MAX_BLOCKS)
			throw new UnityException("Error: Too many terrain blocks");
		int resolution = terrainPrefab.terrainData.heightmapResolution;
		float[,] heights = new float[resolution, resolution];
		for (int i = 0; i < heights.GetLength(0); i++)
		{
			for (int j = 0; j < heights.GetLength(1); j++)
			{
				int x = xShift * resolution + i;
				int y = yShift * resolution + j;
				heights[i, j] = 0.1f * SimplexNoise.Noise.Generate(x / 100.0f, y / 100.0f) + 0.5f;
			}
		}

		Terrain terrain = Instantiate(terrainPrefab) as Terrain;
		terrain.terrainData = Instantiate(terrainPrefab.terrainData) as TerrainData;
		//terrain.terrainData.heightmapResolution = resolution;
		terrain.terrainData.SetHeights(0, 0, heights);
		terrain.transform.position += new Vector3(yIndex * blockSize, 0.0f, xIndex * blockSize);

		PlaceTrees(terrain, 1000);

		return terrain;
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
			tree.position = new Vector3(Random.value, 0.0f, Random.value);
			terrain.AddTreeInstance(tree);
		}
	}
}
