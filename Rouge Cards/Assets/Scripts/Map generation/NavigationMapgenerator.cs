using UnityEngine;
using System.Collections.Generic;

public class NavigationMapGenerator : MonoBehaviour
{
    private List<TileData> currenttileDatas;
    private List<TileData> nexttileDatas;
    private float currentnavigationalvalue;
    private LevelGeneration_Plains levelgenerator;

    private void Start()
    {
        levelgenerator = FindAnyObjectByType<LevelGeneration_Plains>();
    }
    public void GenerateNavigationMap(TileData destination)
    {
        currenttileDatas = new List<TileData>();
        nexttileDatas = new List<TileData>();
        currentnavigationalvalue = 0;

        // Reset all tiles if needed (optional: do this in a manager class)

        // Initialize destination tile
        destination.NavigationValue = 0;
        destination.NavigationSet = true;
        currenttileDatas.Add(destination);

        // Start processing the map
        ProcessNavigationMap();
    }

    private void ProcessNavigationMap()
    {
        while (currenttileDatas.Count > 0)
        {
            nexttileDatas.Clear();

            foreach (TileData tile in currenttileDatas)
            {
                List<TileData> neighbors = tile.GetNeighbors();

                foreach (TileData neighbor in neighbors)
                {
                    if (!neighbor.NavigationSet)
                    {
                        neighbor.NavigationSet = true;
                        neighbor.NavigationValue = tile.NavigationValue + 1;
                        nexttileDatas.Add(neighbor);
                    }
                }
            }

            // Move to next "layer"
            currenttileDatas.Clear();
            currenttileDatas.AddRange(nexttileDatas);
            currentnavigationalvalue++;
        }
    }
    public void clearalltiles()
    {
        for (int x = 0; x < levelgenerator.TileList_Width.Count; x++)
        {
            for (int y = 0; y < levelgenerator.TileList_Width[x].Count; y++)
            {
                TileData tile = levelgenerator.TileList_Width[x][y].GetComponent<TileData>();
                if (tile != null)
                {
                    tile.NavigationValue = 0;
                    tile.NavigationSet = false;
                }
            }
        }

    }
}