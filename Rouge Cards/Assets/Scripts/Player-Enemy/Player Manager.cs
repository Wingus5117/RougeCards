using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject SelectedObject;

    // Update is called once per frame
    void Update()
    {
        SelectObject();
    }
    public void SelectObject()
    {
        // if press left mouse
        if (Input.GetMouseButtonDown(0))
        {
            //send out a raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // check if it hits something
            if (Physics.Raycast(ray, out hit))
            {
                //what did we hit

                //if we hit a tile get its data
                if (hit.collider.gameObject.tag == "Tile")
                {
                    TileData data = hit.collider.gameObject.GetComponent<TileData>();
                    
                    // if we are not currently selected on something make the Tile the selected object
                    if (SelectedObject == null)
                    {
                        SelectedObject = hit.collider.gameObject;
                    }
                    // if we currently have a player unit selected that means we try to move to that tile
                    else if (SelectedObject.tag == "PlayerUnit")
                    {
                        // make sure that the selected tile is a valid target for movement
                        
                        PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
                        // if the player unit can fly let them land on any tile exept those with enemies or mountian tiles
                        if (gameplayobject.isFlying)
                        {
                            if (data.Terraintype == TerrainType.Mountain || data.ObjectOnTile != null)
                            {
                                Debug.Log("Target Tile is not a valid target");
                                return;
                            }
                        }
                        //if the player unit CANNOT fly let them land on any tile exept those with enemies,mountain or water tiles
                        else
                        {
                            if (data.Terraintype == TerrainType.Mountain || data.Terraintype == TerrainType.Water || data.ObjectOnTile != null)
                            {
                                Debug.Log("Target Tile is not a valid target");
                                return;
                            }
                        }
                        
                        //if the tile is a valid movement location try to move to it and make the tile the selected object
                        gameplayobject.Move(data);
                        gameplayobject.isSelected = false;
                        SelectedObject = hit.collider.gameObject;
                    }
                }
                // if we click on a player unit at any time make the unit the selected object
                else if (hit.collider.gameObject.tag == "PlayerUnit")
                {
                    SelectedObject = hit.collider.gameObject;
                    PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
                    gameplayobject.Select();
                }
                // if we click on an enemy unit 
                else if (hit.collider.gameObject.tag == "EnemyUnit")
                {
                    
                    // if we are selected onto nothing or a tile then make the enemy the selected object
                    if (SelectedObject == null || SelectedObject.gameObject.tag == "Tile")
                    {
                        SelectedObject = hit.collider.gameObject;
                    }
                    // if we are currently selected onto a player unit and click on an enemy we need to move to them and then initiate combat
                    else if (SelectedObject.tag == "PlayerUnit")
                    {
                        // get the info of the enemy we click on and the tile they are on
                        EnemyUnit enemyunit = hit.collider.gameObject.GetComponent<EnemyUnit>();
                        TileData data = enemyunit.TilePosition.GetComponent<TileData>();
                        PlayerUnit playerunit = SelectedObject.GetComponent<PlayerUnit>();

                        // tell the player unit to move to the tile that the enemy unit is on
                        // (THIS IS WRONG AND WILL FAIL TO MOVE THE UNIT SINCE THE TILE IS OCCUPIED, we need to try to move untill we are in attack range or make moving and attacking seperate)
                        //playerunit.Move(data);
                        // make the enemy the selected object
                        Debug.Log("GOTO" +  data.gameObject.name);
                        SelectedObject = enemyunit.gameObject;
                    }

                    // make the enemy to currently selected object
                    SelectedObject = hit.collider.gameObject;

                }
            }
        }
    }

    public void InitiateCombat()
    { 
    
    }
}
