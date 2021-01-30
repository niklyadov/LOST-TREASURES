using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int Depth = 20;
    public int Size = 500;
    public float DetailedScale = 100;
    public float SmallScale = 40;
    public float Scale = 20;
    public float BigScale = 5;
    public float LargeScale = 3;


    public float offsetX = 100f;
    public float offsetY = 100f;

    private Terrain _terrain;

    private void Start()
    {
        _terrain = GetComponent<Terrain>();
        _terrain.terrainData = GenerateTerrain(_terrain.terrainData);

    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = Size + 1;
        terrainData.size = new Vector3(Size, Depth, Size);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        var heights = new float[Size, Size];

        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                heights[x, y] = CalculateHeight(x, y);

        return heights;
    }

    private float CalculateHeight(float x, float y)
    {
        //var noise = CalculatePerlinNoise(x, y, BigScale);
        //if (noise < 0.25f)
        //    return noise * 3;
        //else
        //    return 1 - noise;


        return 0.6f*( 0.8f*CalculatePerlinNoise(x,y,Scale) - CalculatePerlinNoise(x,y,BigScale) + 0.1f*CalculatePerlinNoise(x, y, SmallScale));
    }

    private float CalculatePerlinNoise(float x, float y, float scale)
    {
        return Mathf.PerlinNoise
            (
                x / Size * scale + offsetX,
                y / Size * scale + offsetY
            );
    }
}
