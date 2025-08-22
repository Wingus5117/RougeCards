
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
public class LevelGeneration : MonoBehaviour
{
    public MapBiome MapBiome;
    
    public Vector2 TileMap_WidthBounds;
    public Vector2 TileMap_LengthBounds;

    public int Seed;
    
    public GameObject Tile;

    public Material GrassMaterial;
    public Material MountainMaterial;
    public Material WaterMaterial;

    public List<List<GameObject>> TileList_Width = new List<List<GameObject>>();
    public int TileAmount = 0;

    public bool BaseTilesGenerated;
    public bool MainTileTypePlaced;
    public bool SecondaryTileTypePlaced;
    public bool TertiaryTileTypePlaced;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartMapGeneration();
        }
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
                TileList_Width[i].Add(Tile);
                
                GameObject tile = Instantiate(Tile,  new Vector3(j,0,i), Quaternion.identity, transform);
                
                MeshRenderer mr = tile.GetComponent<MeshRenderer>();
                mr.material = GrassMaterial;

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
    public IEnumerator StartMapGeneration()
    {
        GenerateBaseTileMap();
        while (!BaseTilesGenerated)
        {
            return null;
        }
        Debug.Log("BaseTilesPlaced");
        return null;
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


