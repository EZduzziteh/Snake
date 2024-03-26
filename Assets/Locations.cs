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

   

    List<List<Tile>> gridColumns = new List<List<Tile>>();

    [SerializeField]
    
    
    
    Tile tile;

    public void Fixup()
    {

        //first set all tiles to not occupied
        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                gridColumns[i][j].isOccupied = false;
            }
        }

        foreach (var snake in FindObjectsOfType<Snake>())
        {
            //force snakes to occupy their tiles
            snake.OccupyAllTiles();
        }
    }


    public Tile GetStartTile()
    {
        //return the center tile
        return gridColumns[gridSizeY / 2][gridSizeX / 2];
    }

    public Tile GetRandomUnoccupiedTile()
    {
        int maxAttempts = gridSizeX * gridSizeY * 2;
        while (maxAttempts > 0)
        {
            int randomY = Random.Range(0, gridSizeY - 1);
            int randomX = Random.Range(0, gridSizeX - 1);

            if (gridColumns[randomY][randomX].isOccupied == false)
            {
                Debug.Log("found tile"+ gridColumns[randomY][randomX].gameObject.name);
                return gridColumns[randomY][randomX];
            }
            maxAttempts --;
        }
        //if we cant find a tile after a certain amount of tries, return null. 

        return null;
    }

    void Start()
    {
        //this creates all of the tiles
        for(int i = 0; i < gridSizeY; i++)
        {
            List<Tile> gridRow = new List<Tile>();

            for (int j = 0; j < gridSizeX; j++)
            {
                //create world point
                Vector2 newPoint = new Vector2(i * spaceBetween, j * spaceBetween);
                Tile temp = GameObject.Instantiate(tile, new Vector3(newPoint.x, newPoint.y, 0), Quaternion.identity);
                temp.name = "X: "+i+", Y: "+j;
                gridRow.Add(temp);
                
            }

            gridColumns.Add(gridRow);
        }


        Debug.Log(gridColumns.Count);
        Debug.Log(gridColumns[3][3].gameObject.name);
        //once we have all the tiles generated, we can loop through all the tiles and assign them the tiles adjacent to them.
        for(int i = 0; i < gridSizeY; i++)
        {
            //foreach column


            for(int j = 0; j<gridSizeX; j++)
            {

                Debug.Log("for: "+i+", "+j);

                //i == y axis
                // j == x axis


                //first check if we are on any edges
                //then assign tiles accordingly

                if(i != 0)
                {
                    //if we are not at the bottom, assign the below tile.
                    gridColumns[i][j].tile_left = gridColumns[i - 1][j];
                    Debug.Log("assign bottom");
                }
                else
                {
                    Debug.Log("at bottom");
                }
                if (i != gridSizeY - 1)
                {
                    //if we are not at the top, assign the above tile.
                    gridColumns[i][j].tile_right = gridColumns[i + 1][j];
                }
                else
                {
                    Debug.Log("at top");
                }
                if (j != 0)
                {
                    //if we are not on the left edge
                    gridColumns[i][j].tile_down = gridColumns[i][j - 1];
                }
                else
                {
                    Debug.Log("at left");
                }
                if (j!= gridSizeX - 1)
                {
                    //if we are not on the right edge
                    gridColumns[i][j].tile_up = gridColumns[i][j + 1];
                    
                }
                else
                {
                    Debug.Log("at right");
                }

            }
        }


        //set camera to be in the middle of the board we just generated
        Camera.main.transform.position = new Vector3(gridSizeX / 2 * spaceBetween, gridSizeY / 2 * spaceBetween, Camera.main.transform.position.z );

      
    }


   
}
