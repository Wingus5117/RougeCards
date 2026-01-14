using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class NavigationMapGenerator : MonoBehaviour
{
    private List<TileData> currenttileDatas;
    private List<TileData> nexttileDatas;
    private float currentnavigationalvalue;
    private MapManager levelgenerator;
    internal bool CurrentUnitisFlying;
    public TurnManager TurnManager;

    //public Material IndicatorRed;
    //public Material IndicatorBlue;
    //public Material IndicatorPurple;

    private void Start()
    {
        levelgenerator = FindAnyObjectByType<MapManager>();
    }
    public void GenerateNavigationMap(TileData destination, TileData startTile)
    {
        currenttileDatas = new List<TileData>();
        nexttileDatas = new List<TileData>();
        currentnavigationalvalue = 0;


        
        //set the tile the unit is on as the inital tile to generate the navmesh from
        //currenttileDatas.Add(startTile);

        //startTile.NavigationValue = 0;
        //startTile.NavigationSet = true;

        // Initialize destination tile
        destination.NavigationValue = 0;
        destination.NavigationSet = true;

        //set the destination as the inital tile that we propogate the navmesh from
        currenttileDatas.Add(destination);

        // Start processing the map
        ProcessNavigationMap(destination);
    }

    private void ProcessNavigationMap(TileData destination)
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
        // once the map is fully done generating reset the flying bollean to flase
        CurrentUnitisFlying = false;
    }
    /*public void HighlightValidTiles(TileData PlayerTile)
    {
        PlayerUnit playerUnit = PlayerTile.ObjectOnTile.GetComponent<PlayerUnit>();
        
        List<TileData> currenttileDatas = new List<TileData>();
        List<TileData> nexttileDatas = new List<TileData>();

        // set the tile's indicator the player is on to purple
        PlayerTile.TurnOnIndicator(IndicatorBlue);
        PlayerTile.IndicatorSet = true;

        currenttileDatas.Add(PlayerTile);

       
        float MoveRange = PlayerTile.ObjectOnTile.GetComponent<PlayerUnit>().MovementPoints;

        while (currenttileDatas.Count > 0)
        {
            nexttileDatas.Clear();

            
            MoveRange--;
            if (MoveRange < 0)
            {
                currenttileDatas.Clear();
                return;
            }
            

            foreach (TileData tile in currenttileDatas)
            {
                List<TileData> neighbors = tile.GetNeighbors();

                foreach (TileData neighbor in neighbors)
                {
                    if (!neighbor.IndicatorSet)
                    {

                        if (neighbor.Terraintype == TerrainType.Grass)
                        {
                            // can move to tile but not attack
                            if (MoveRange >= 0)
                            {
                                neighbor.TurnOnIndicator(IndicatorBlue);
                            }
                        }
                        if (neighbor.Terraintype == TerrainType.Water)
                        {
                            if (playerUnit.isFlying)
                            {
                                // can move to tile but not attack
                                if (MoveRange >= 0)
                                {
                                    neighbor.TurnOnIndicator(IndicatorBlue);
                                }
                                
                            }
                            
                        }
                        
                    }
                        
                        //activate the indicator based on ht emovment and attack range values
                        nexttileDatas.Add(neighbor);
                    
                }
            }
            // Move to next "layer"
            currenttileDatas.Clear();
            currenttileDatas.AddRange(nexttileDatas);
        }
        HilightAttackTiles(PlayerTile);
        // once the map is fully done generating reset the flying bollean to flase
    }
    public void HilightAttackTiles(TileData PlayerTile)
    {
        List<TileData> currenttileDatas = new List<TileData>();
        List<TileData> nexttileDatas = new List<TileData>();

        PlayerUnit playerUnit = PlayerTile.ObjectOnTile.GetComponent<PlayerUnit>();

        float AttackRange = playerUnit.AttackRange + playerUnit.MovementPoints;
        currenttileDatas.Add(PlayerTile);

        while (currenttileDatas.Count > 0)
        {
            nexttileDatas.Clear();


            AttackRange--;
            if (AttackRange < 0)
            {
                currenttileDatas.Clear();
                return;
            }


            foreach (TileData tile in currenttileDatas)
            {
                List<TileData> neighbors = tile.GetNeighbors();

                foreach (TileData neighbor in neighbors)
                {
                    if (!neighbor.IndicatorSet)
                    {
                        if (AttackRange >= 0)
                        {
                            Debug.Log("RED");
                            neighbor.TurnOnIndicator(IndicatorRed);
                        }


                    }

                    //activate the indicator based on ht emovment and attack range values
                    nexttileDatas.Add(neighbor);

                }
            }
            // Move to next "layer"
            currenttileDatas.Clear();
            currenttileDatas.AddRange(nexttileDatas);
        }
    }*/

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

                    //tile.WalkIndicator.gameObject.SetActive(false);
                    //tile.IndicatorSet = false;

                }
            }
        }

    }

}