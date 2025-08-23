using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public TerrainType Terraintype;
    public GameObject ObjectOnTile;
}
public enum TerrainType
{
    Grass,
    Water,
    Mountain,
    Mana,

}
