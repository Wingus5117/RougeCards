using UnityEngine;

public class GamePlayObject : MonoBehaviour
{
    public bool isSelected;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTilePosition();
    }
    public void UpdateTilePosition()
    {
        // Origin of the ray
        Vector3 origin = transform.position;

        // Direction of the ray (downwards)
        Vector3 direction = Vector3.down;

        // Visualize the ray in the Scene view
        Debug.DrawRay(origin, direction * 2, Color.red);

        // Perform the raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 2))
        {
            TileData tiledata = hit.collider.gameObject.GetComponent<TileData>();
            tiledata.ObjectOnTile = gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Select()
    {
        isSelected = true;
        
    }
    public void UnSelect()
    {
        isSelected = false;
    }
    public void Move(TileData destination)
    {
        Debug.Log("GoTo " +  destination);
        generateNavigationGrid(destination);
    }
    public void generateNavigationGrid(TileData destination)
    {
        StartCoroutine(destination.GenerateNavigationMapOrigin());
    }
}
