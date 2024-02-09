using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locations : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    int gridSizeX = 10;
    [SerializeField]
    int gridSizeY = 10;

    float spaceBetween = 0.5f;

    List<Vector2> gridX = new List<Vector2>();

    List<List<Vector2>> gridY = new List<List<Vector2>>();

    [SerializeField]
    GameObject tile;

    void Start()
    {
        for(int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                //create world point
                Vector2 newPoint = new Vector2(i * spaceBetween, j * spaceBetween);
                gridX.Add(newPoint);
                GameObject.Instantiate(tile, new Vector3(newPoint.x, newPoint.y, 0), Quaternion.identity);
                
            }
        }
      
    }


   
}
