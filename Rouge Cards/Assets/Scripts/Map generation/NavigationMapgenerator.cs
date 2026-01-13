using UnityEngine;
using System.Collections.Generic;

public class NavigationMapGenerator : MonoBehaviour
{
    private List<TileData> currenttileDatas;
    private List<TileData> nexttileDatas;
    private float currentnavigationalvalue;
    private MapManager levelgenerator;
    internal bool CurrentUnitisFlying;
    public TurnManager TurnManager;

    private void Start()
    {
        levelgenerator = FindAnyObjectByType<MapManager>();
    }
    public void GenerateNavigationMap(TileData destination)
    {
        currenttileDatas = new List<TileData>();
        nexttileDatas = new List<TileData>();
        currentnavigationalvalue = 0;


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
                        // if unit can fly add all tiles to the navigation grid
                        if (CurrentUnitisFlying)
                        {
                            neighbor.NavigationSet = true;
                            neighbor.NavigationValue = tile.NavigationValue + 1;
                            nexttileDatas.Add(neighbor);
                        }
                        // check if the next tile is walkable for non flying units
                        else if (neighbor.Terraintype.ToString() == "Grass" || neighbor.Terraintype.ToString() == "Mana")
                        {
                            if (TurnManager.isPlayerTurn)
                            {
                                if (!neighbor.hasEnemyEntity)
                                {
                                    neighbor.NavigationSet = true;
                                    neighbor.NavigationValue = tile.NavigationValue + 1;
                                    nexttileDatas.Add(neighbor);
                                }
                            }
                            if (TurnManager.isEnemyTurn)
                            {
                                if (!neighbor.hasPlayerEntity)
                                {
                                    neighbor.NavigationSet = true;
                                    neighbor.NavigationValue = tile.NavigationValue + 1;
                                    nexttileDatas.Add(neighbor);
                                }
                            }
                            // if the tile has an enemy on it dont add it to the navigation grid
                            
                        }
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