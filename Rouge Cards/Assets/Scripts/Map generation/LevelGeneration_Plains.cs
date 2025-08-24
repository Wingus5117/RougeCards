
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum MapBiome
{
    Plains,
    Mountain,
    Lake,
    Islands,

}
[Serializable]
public class LevelGeneration_Plains : MonoBehaviour
{
    public MapBiome MapBiome;
    public BiomeData PlainsData;
    
    public Vector2 TileMap_WidthBounds;
    public Vector2 TileMap_LengthBounds;

    public int Seed;
    
    public GameObject Tile;

    public Material GrassMaterial;
    public Material ForestMaterial;
    public Material WaterMaterial;

    public List<List<GameObject>> TileList_Width = new List<List<GameObject>>();
    public int TileAmount = 0;

    public bool BaseTilesGenerated;
    public bool MainTileTypePlaced;
    public bool SecondaryTileTypePlaced;
    public bool TertiaryTileTypePlaced;

    public float primaryTileBudget;
    public float secondaryTileBudget;
    public float tertiaryTileBudget;

    public float LakeBudget;
    public float RiverBudget;
    public float forestBudget;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateMap();
        }
    }
    public IEnumerator GenerateMap()
    {
        
        GenerateBaseTileMap();
        DetermineTilebudgets();
        while (!BaseTilesGenerated)
        {
            return null;
        }
        Debug.Log("BaseTilesPlaced");
        
        return null;
    }
    
    public void GenerateBaseTileMap()
    {
        //Reset
        TileAmount = 0; 
        TileList_Width.Clear();
        foreach(Transform child in transform)
        { 
            Destroy(child.gameObject);
        }

        //Set the seed
        UnityEngine.Random.InitState(Seed);
        
        //Decide how wide and long the map is
        float TileMap_Width = UnityEngine.Random.Range(TileMap_WidthBounds.x, TileMap_WidthBounds.y);
        TileMap_Width = Mathf.FloorToInt(TileMap_Width);
        float TileMap_Length = UnityEngine.Random.Range(TileMap_LengthBounds.x, TileMap_LengthBounds.y);
        TileMap_Length = Mathf.FloorToInt(TileMap_Length);
        //Debug.Log("Length: " + TileMap_Length + "Width: " + TileMap_Width);

        for (int i = 0; i < TileMap_Width; i++)
        {
            //each iteration of this adds a new length list
            List<GameObject> newRow = new List<GameObject>();
            TileList_Width.Add(newRow);
            //Debug.Log("Tile map updated. Current rows: " + TileList_Width.Count);
        }
        //Iterate through all tile slots lengthwise
        for (int i = 0; i < TileMap_Width; i++)
        {
            //add to each list the amount of tiles in the width
            for (int j = 0; j < TileMap_Length; j++)
            {
                
                
                GameObject tile = Instantiate(Tile,  new Vector3(j,0,i), Quaternion.identity, transform);

                TileList_Width[i].Add(tile);

                tile.name = (i + "," + j);
               
                
                //tells the tile what position it is
                TileData tileData = tile.GetComponent<TileData>();
                tileData.Xposition = i;
                tileData.Yposition = j;
                
                //Sets the current tile to the biomes base tile
                PlacePrimaryTileType(tile);

                //this commented out section would make each tile randomly water, mountain or grass material
                /*int tiletypevalue = UnityEngine.Random.Range(1, 11);
                Debug.Log(tiletypevalue);
                if (tiletypevalue <= 3)
                { 
                    mr.material = GrassMaterial;
                }
                else if (tiletypevalue > 3 && tiletypevalue < 6) 
                {
                    mr.material = WaterMaterial;
                }
                else if (tiletypevalue >= 6)
                {
                    mr.material = MountainMaterial;
                }*/

                TileAmount++;
            }

        }
        BaseTilesGenerated = true;
       
    }

    public void PlacePrimaryTileType(GameObject tile)
    {
        // set the given tile to the default tile type for the given biome
        MeshRenderer mr = tile.GetComponent<MeshRenderer>();
        mr.material = GrassMaterial;
    }
    public void DetermineTilebudgets()
    {
        BiomeData MapBiomeData = null;
        if (MapBiome == MapBiome.Plains)
        {
            MapBiomeData = PlainsData;
        }

        primaryTileBudget = TileAmount * (MapBiomeData.GrassBudget / 100);
        primaryTileBudget = Mathf.FloorToInt(primaryTileBudget);
        
        secondaryTileBudget = TileAmount * (MapBiomeData.WaterBudget / 100);
        secondaryTileBudget = Mathf.FloorToInt(secondaryTileBudget);
        
        tertiaryTileBudget = TileAmount * (MapBiomeData.MountainBudget / 100);
        tertiaryTileBudget = Mathf.FloorToInt(tertiaryTileBudget);


    }
    public void PlaceSecondarytileType()
    {
        //foreach river in the budget make a river
        for (int i = 0; i < RiverBudget; i++)
        {
            // find an edge tile
            // place a water tile on a random one
            // 
        }
    }
    
    public void CheckNearbyTiles(int SearchRange, String TileTypeToSearchFor)
    {
        // this enum will be retrived from the tiles that are being searched
        Enum TileType;
        // convert the found tiles type to a string
        string test = nameof(TileType);
       
        // check to see if that string is the one we are searching for
        if (test == TileTypeToSearchFor)
        {
            Debug.Log("Found A Water Tile");
        }
    }
}


