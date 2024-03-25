using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    Tile currentTile;
    Locations locationReference;

    private void Start()
    {
        locationReference = FindObjectOfType<Locations>();
    }

    private void Update()
    {
        if(currentTile == null) { 
            MoveToNewTile();
        }
    }
    public void MoveToNewTile()
    {
        if (currentTile)
        {
            currentTile.SetFood(null);
            currentTile.isOccupied = false;
        }
        currentTile = FindObjectOfType<Locations>().GetRandomUnoccupiedTile();
        transform.position = currentTile.transform.position;
        //currentTile.SetFood(this);
        currentTile.food = this;
        Debug.Log(currentTile);
        Debug.Log("has food:" +currentTile.CheckHasFood());
    }
}
