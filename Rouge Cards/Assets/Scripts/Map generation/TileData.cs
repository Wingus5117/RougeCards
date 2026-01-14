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
    public bool hasEnemyEntity;
    public bool hasPlayerEntity;

    public float NavigationValue;
    public bool NavigationSet = false;

    internal MapManager Mapmanager;
    private List<TileData> _tiles = new List<TileData>();
    //internal bool IndicatorSet;
    //public Renderer WalkIndicator;

    public void OnEnable()
    {
        Mapmanager = FindAnyObjectByType<MapManager>();
    }


    // Helper to get adjacent neighbors
    internal List<TileData> GetNeighbors()
    {
        List<TileData> result = new List<TileData>();

        int x = Xposition.ConvertTo<int>();
        int y = Yposition.ConvertTo<int>();

        // Grid dimensions
        int width = Mapmanager.TileList_Width.Count;
        int height = Mapmanager.TileList_Width[0].Count;

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
                TileData neighbor = Mapmanager.TileList_Width[newX][newY].GetComponent<TileData>();
                if (neighbor != null)
                {
                    result.Add(neighbor);
                }
            }
        }

        return result;
    }

    /*public void TurnOnIndicator(Material IndicatorMaterial)
    {
        //Red, attack
        //Blue, move
        //Purple, both
        IndicatorSet = true;
        WalkIndicator.gameObject.SetActive(true);
        WalkIndicator.material = IndicatorMaterial;
    }*/

}
public enum TerrainType
{
    Grass,
    Water,
    Mountain,
    Mana,
}
