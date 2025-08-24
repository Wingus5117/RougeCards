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
    public IEnumerator GenerateNavigationMapOrigin()
    {
        yield return new WaitForSeconds(.001f);
        NavigationValue = 0;
        NavigationSet = true;

        // Get neighbors fresh for this step
        List<TileData> neighbors = GetNeighbors();

        foreach (TileData tile in neighbors)
        {
            if (!tile.NavigationSet)
            {
                Debug.Log("Setting " + tile.name);
                tile.NavigationSet = true;
                tile.NavigationValue = NavigationValue + 1;
                StartCoroutine(tile.generateNavigationMapCont());
            }
        }
    }

    public IEnumerator generateNavigationMapCont()
    {
        yield return new WaitForSeconds(.001f);

        // Fresh neighbor list every time
        List<TileData> neighbors = GetNeighbors();

        foreach (TileData tile in neighbors)
        {
            if (!tile.NavigationSet)
            {
                Debug.Log("Setting " + tile.name);
                tile.NavigationSet = true;
                tile.NavigationValue = NavigationValue + 1;
                StartCoroutine(tile.generateNavigationMapCont());
            }
        }
    }

    // Helper to get adjacent neighbors
    private List<TileData> GetNeighbors()
    {
        List<TileData> result = new List<TileData>();

        for (int i = 0; i < LevelGeneration_Plains.TileList_Width.Count; i++)
        {
            for (int j = 0; j < LevelGeneration_Plains.TileList_Width[i].Count; j++)
            {
                TileData data = LevelGeneration_Plains.TileList_Width[i][j].GetComponent<TileData>();

                if (data.Xposition == Xposition + 1 && data.Yposition == Yposition)
                    result.Add(data);

                if (data.Xposition == Xposition - 1 && data.Yposition == Yposition)
                    result.Add(data);

                if (data.Xposition == Xposition && data.Yposition == Yposition + 1)
                    result.Add(data);

                if (data.Xposition == Xposition && data.Yposition == Yposition - 1)
                    result.Add(data);
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
