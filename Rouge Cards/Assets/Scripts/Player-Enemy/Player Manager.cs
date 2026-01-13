using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject SelectedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
                if (hit.collider.gameObject.tag == "Tile")
                {
                    TileData data = hit.collider.gameObject.GetComponent<TileData>();
                    
                    if (SelectedObject == null)
                    {
                        SelectedObject = hit.collider.gameObject;

                    }
                    else if (SelectedObject.tag == "PlayerUnit")
                    {

                        //Check to see if the targetted tile is a valid target
                        if (data.Terraintype == TerrainType.Mountain || data.Terraintype == TerrainType.Water || data.ObjectOnTile != null)
                        {
                            return;
                        }
                        
                        PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
                        gameplayobject.Move(data);
                        gameplayobject.isSelected = false;
                        SelectedObject = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.gameObject.tag == "PlayerUnit")
                {
                    SelectedObject = hit.collider.gameObject;
                    PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
                    gameplayobject.Select();
                }

            }
        }
    }
    public void UnselectPreviousObject()
    {
        if (SelectedObject.tag == "Tile")
        {


        }
        else if (SelectedObject.tag == "PlayerUnit")
        {
            PlayerUnit gameplayobject = SelectedObject.GetComponent<PlayerUnit>();
            gameplayobject.UnSelect();
        }
    }
}
