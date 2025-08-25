using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public TerrainType Terraintype;
    public float Xposition;
    public float Yposition;
    public GameObject ObjectOnTile;

    public float NavigationValue;
    public bool NavigationSet = false;

    internal LevelGeneration_Plains LevelGeneration_Plains;
    private List<TileData> _tiles = new List<TileData>();

    public void OnEnable()
    {
        LevelGeneration_Plains = FindAnyObjectByType<LevelGeneration_Plains>();
    }
    

    // Helper to get adjacent neighbors
    internal List<TileData> GetNeighbors()
    {
        List<TileData> result = new List<TileData>();

        int x = Xposition.ConvertTo<int>();
        int y = Yposition.ConvertTo<int>();

        // Grid dimensions
        int width = LevelGeneration_Plains.TileList_Width.Count;
        int height = LevelGeneration_Plains.TileList_Width[0].Count;

        // Define offsets for 4 directions: up, down, left, right
        int[,] directions = new int[,]
        {
        { 0, 1 },  // Up
        { 0, -1 }, // Down
        { 1, 0 },  // Right
        { -1, 0 }  // Left
        };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + directions[i, 0];
            int newY = y + directions[i, 1];

            // Bounds check
            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                TileData neighbor = LevelGeneration_Plains.TileList_Width[newX][newY].GetComponent<TileData>();
                if (neighbor != null)
                {
                    result.Add(neighbor);
                }
            }
        }

        return result;
    }
}
public enum TerrainType
{
    Grass,
    Water,
    Mountain,
    Mana,
}
