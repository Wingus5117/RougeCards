using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
public class PlayerUnit : MonoBehaviour
{
    public bool isSelected;
    public TileData TilePosition;
    private NavigationMapGenerator NavigationMapgenerator;
    private bool ismoving;
    public bool isFlying;

    //Unit Stats
    public string UnitName;
    public float ManaCost;
    public float Movement;
    public float MovementPoints;
    public float ActionPoints;
    public float MaxHealth;
    public float CurrentHealth;
    public float Damage;
    public float AttackRange;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTilePosition();
        NavigationMapgenerator = FindAnyObjectByType<NavigationMapGenerator>();
    }
    public void UpdateTilePosition()
    {
        Debug.Log("UpdateTile");
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
    public void Select()
    {
        UpdateTilePosition();
        isSelected = true;
        NavigationMapgenerator.HighlightValidTiles(TilePosition);
    }
    public void UnSelect()
    {
        isSelected = false;
    }
    public void Move(TileData destination)
    {
        Debug.Log("GoTo " +  destination);
        TilePosition.ObjectOnTile = null;
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

            // Move the object 1 unit toward the best neighbor tile
            StartCoroutine(MoveToTile(bestNeighbor, destination));
            MovementPoints--;
            // Update the new tile
            TilePosition = bestNeighbor;
        }
        else
        {
            Debug.Log("No Valid Path");
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

        
        if (TilePosition != Destination)
        {
            MoveOneStepTowardsTarget(Destination);
            Debug.Log("KeepMoving");
        }
        else
        {
            NavigationMapgenerator.clearalltiles();
            Debug.Log("StopMoving");
            // Update tile references after movement finishes
            TilePosition.ObjectOnTile = null;
            TilePosition = targetTile;
            targetTile.ObjectOnTile = gameObject;

        }
    }
    public void generateNavigationGrid(TileData destination)
    {
        //make sure the navigation map generator knows if the unit that is trying to move can fly or not
        NavigationMapgenerator.CurrentUnitisFlying = isFlying;
        NavigationMapgenerator.GenerateNavigationMap(destination, TilePosition);
    }

    public void TakeDamage(float Damagetaken)
    { 
        CurrentHealth -= Damagetaken;
        if (CurrentHealth <= 0)
        {
            OnDeath();
        }
    }
    public void OnDeath()
    {
        Destroy(gameObject, 1);
    }

    public void NewPlayerTurn()
    {
        MovementPoints = Movement;
        ActionPoints = 1;
    }
}
