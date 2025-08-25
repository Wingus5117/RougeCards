using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class GamePlayObject : MonoBehaviour
{
    public bool isSelected;
    public TileData TilePosition;
    private NavigationMapGenerator NavigationMapgenerator;
    private bool ismoving;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTilePosition();
        NavigationMapgenerator = FindAnyObjectByType<NavigationMapGenerator>();
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
            TilePosition = tiledata;
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
        MoveOneStepTowardsTarget(destination);
    }
    public void MoveOneStepTowardsTarget(TileData destination)
    {
        if (TilePosition == null) return;

        float currentValue = TilePosition.NavigationValue;
        List<TileData> neighbors = TilePosition.GetNeighbors();

        TileData bestNeighbor = null;
        float lowestValue = currentValue;

        foreach (TileData tile in neighbors)
        {
            
            if (tile.NavigationSet && tile.NavigationValue < lowestValue)
            {
                lowestValue = tile.NavigationValue;
                bestNeighbor = tile;
                
            }
        }

        
        if (bestNeighbor != null)
        {
            // Optional: Clear current tile's object reference
            TilePosition.ObjectOnTile = null;

            
            // Move the object 1 unit toward the best neighbor tile
            StartCoroutine(MoveToTile(bestNeighbor, destination));

            // Update the new tile
            TilePosition = bestNeighbor;
            bestNeighbor.ObjectOnTile = gameObject;
        }
        
    }
    private IEnumerator MoveToTile(TileData targetTile,TileData Destination, float duration = 0.25f)
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(
            targetTile.transform.position.x,
            transform.position.y,
            targetTile.transform.position.z
        );

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        // Update tile references after movement finishes
        TilePosition.ObjectOnTile = null;
        TilePosition = targetTile;
        targetTile.ObjectOnTile = gameObject;
        if (TilePosition != Destination)
        {
            MoveOneStepTowardsTarget(Destination);
            Debug.Log("KeepMoving");
        }
        else
        {
            NavigationMapgenerator.clearalltiles();
            Debug.Log("StopMoving");
        }
    }
    public void generateNavigationGrid(TileData destination)
    {
        NavigationMapgenerator.GenerateNavigationMap(destination);
    }
}
