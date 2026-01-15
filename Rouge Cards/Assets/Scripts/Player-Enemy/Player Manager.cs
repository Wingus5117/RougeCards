using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject SelectedObject;
    public NavigationMapGenerator NavigationMapGenerator;
    public Material WalkIndicatorMaterial;
    public Material BothIndicatorMaterial;
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

                //if we hit a tile get its data
                if (hit.collider.gameObject.tag == "Tile")
                {
                    //Clear Unit Indicators
                    


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
                        if (data.WalkIndicator.sharedMaterial == WalkIndicatorMaterial || data.WalkIndicator.sharedMaterial == BothIndicatorMaterial)
                        {
                            gameplayobject.Move(data);
                            gameplayobject.isSelected = false;
                            SelectedObject = hit.collider.gameObject;

                        }

                        
                        
                    }
                }
                // if we click on a player unit at any time make the unit the selected object
                else if (hit.collider.gameObject.tag == "PlayerUnit")
                {
                    //if we click on the unit we are already clicked on unselect him and turn off indicators
                    if (SelectedObject == hit.collider.gameObject)
                    {
                        //Clear Unit Indicators
                        NavigationMapGenerator.clearalltiles();
                        PlayerUnit playerunit = SelectedObject.GetComponent<PlayerUnit>();
                        playerunit.isSelected = false;
                        SelectedObject = null;
                        return;

                    }
                    
                    //Clear Unit Indicators
                    NavigationMapGenerator.clearalltiles();


                    SelectedObject = hit.collider.gameObject;
                    PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
                    gameplayobject.Select();
                }
                // if we click on an enemy unit 
                else if (hit.collider.gameObject.tag == "EnemyUnit")
                {
                    //Clear Unit Indicators
                    NavigationMapGenerator.clearalltiles();


                    // if we are selected onto nothing or a tile then make the enemy the selected object 
                    if (SelectedObject == null || SelectedObject.gameObject.tag == "Tile")
                    {
                        // do nothing
                    }
                    // if we are currently selected onto a player unit and click on an enemy we need to check if we are in attack range and then initiate combat
                    else if (SelectedObject.tag == "PlayerUnit")
                    {

                        // get the info of the enemy we click on 
                        EnemyUnit enemyunit = hit.collider.gameObject.GetComponent<EnemyUnit>();
                        PlayerUnit playerunit = SelectedObject.GetComponent<PlayerUnit>();

                        //Check the distance between the player unit and the enemy unit
                        float distance = Vector3.Distance(playerunit.transform.position, enemyunit.transform.position);
                        Debug.Log(distance);
                        // if the distance is not greater than the attack range then hit them
                        if (distance <= playerunit.AttackRange)
                        {
                            // Initiate conbat with the enemy
                            InitiateCombat(enemyunit, playerunit);
                        }
                        else
                        {
                            Debug.Log("Enemy is out of range");
                        }

                        
                        
                    }

                    // make the enemy to currently selected object
                    SelectedObject = hit.collider.gameObject;

                }
            }
        }
    }

    public void InitiateCombat(EnemyUnit Enemy, PlayerUnit Player)
    {
        Player.MovementPoints = 0;
        Player.ActionPoints--;
        Enemy.TakeDamage(Player.Damage);
    }
}
