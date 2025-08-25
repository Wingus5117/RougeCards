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

                    if (SelectedObject == null)
                    {
                        SelectedObject = hit.collider.gameObject;

                    }
                    else if (SelectedObject.tag == "GamePlayObject")
                    {
                        
                        GamePlayObject gameplayobject = SelectedObject.GetComponent<GamePlayObject>();
                        TileData data = hit.collider.gameObject.GetComponent<TileData>();
                        gameplayobject.Move(data);
                        gameplayobject.isSelected = false;
                        SelectedObject = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.gameObject.tag == "GamePlayObject")
                {
                    SelectedObject = hit.collider.gameObject;
                    GamePlayObject gameplayobject = SelectedObject.GetComponent<GamePlayObject>();
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
        else if (SelectedObject.tag == "GamePlayObject")
        {
            GamePlayObject gameplayobject = SelectedObject.GetComponent<GamePlayObject>();
            gameplayobject.UnSelect();
        }
    }
}
