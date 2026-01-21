
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public enum MapBiome
{
    Plains,
    Mountain,
    Lake,
    Islands,

}
[Serializable]
public class MapManager : MonoBehaviour
{
    public GameObject Tile;

    public Material GrassMaterial;
    public Material MountainMaterial;
    public Material WaterMaterial;
    public Material ManaMaterial;

    public List<List<GameObject>> TileList_Width = new List<List<GameObject>>();
    public int TileAmount = 0;

    public bool Tiles_Sorted;
    public TileMapData CurrentTileMapData;
    public GameObject[] AllTiles;
    // Update is called once per frame

    private void Start()
    {
        //Get acsess to the tilemapdata IE:Width and Length
        CurrentTileMapData = FindAnyObjectByType<TileMapData>();

        //Get all the tiles in the scene onto a list
        AllTiles = GameObject.FindGameObjectsWithTag("Tile");


        for (int i = 0; i < CurrentTileMapData.MapWidth; i++)
        {
            //each iteration of this adds a new length list
            List<GameObject> newRow = new List<GameObject>();
            TileList_Width.Add(newRow);
            //Debug.Log("Tile map updated. Current rows: " + TileList_Width.Count);
        }
        //Iterate through all tile slots lengthwise
        for (int i = 0; i < CurrentTileMapData.MapWidth; i++)
        {
            //add to each list the amount of tiles in the width
            for (int j = 0; j < CurrentTileMapData.MapLength; j++)
            {
                
                foreach (GameObject TileID in AllTiles)
                {
                    if (TileID.name == (i + "," + j))
                    {
                        //Add the tile to the list
                        GameObject tile = TileID;
                        TileList_Width[i].Add(tile);
                        TileAmount++;
                        //tells the tile what position it is
                        TileData tileData = tile.GetComponent<TileData>();
                        tileData.Xposition = i;
                        tileData.Yposition = j;

                        Renderer Mesh = tile.GetComponent<Renderer>();
                        if (Mesh.sharedMaterial == GrassMaterial)
                        {
                            tileData.Terraintype = TerrainType.Grass;
                        }
                        else if (Mesh.sharedMaterial == WaterMaterial)
                        {
                            tileData.Terraintype = TerrainType.Water;
                        }
                        else if (Mesh.sharedMaterial == MountainMaterial)
                        {
                            tileData.Terraintype = TerrainType.Mountain;
                        }
                        else if (Mesh.sharedMaterial == ManaMaterial)
                        {
                            tileData.Terraintype = TerrainType.Mana;
                        }

                    }
                }
                
            }

        }
        Tiles_Sorted = true;
    }






    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateMap();
        }
    }
    public IEnumerator GenerateMap()
    {
        
        GenerateBaseTileMap();
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
                }

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
    }*/
}


