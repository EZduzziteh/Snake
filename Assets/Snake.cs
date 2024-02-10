using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum_Direction;

public class Snake : MonoBehaviour
{
    List<Snake_Body> snakeBody = new List<Snake_Body>();
    [SerializeField]
    float timeBetweenMoves = 1.0f;
   
    direction currentMovingDirection = direction.down;
    float timeSinceLastMove = 0.0f;

    Tile currentTile;
    bool isAlive = false;

    [SerializeField]
    Color snakeColor = Color.blue;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       

        if (isAlive)
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove >= timeBetweenMoves)
            {
                Move();
            }
        }
    }

    public void Die()
    {
        Debug.Log("dead");
    }

    public void Respawn()
    {
        if (!isAlive)
        {
            if (currentTile)
            {
                currentTile.isOccupied = false;
            }

            currentTile = FindAnyObjectByType<Locations>().getStartTile();
            transform.position = currentTile.transform.position;
            currentTile.isOccupied = true;
            Debug.Log("start");
            isAlive = true;
        }
    }

    public void Move()
    {
        if (isAlive) { 
            if (currentTile)
            {
                if (snakeBody.Count <= 0)
                {

                    currentTile.isOccupied = false;
                }

                ////start at the rear: move to equal the position of the next,
                //when we reach the front, move based on current direction.
                //


                switch (currentMovingDirection)
                {
                    case direction.up:
                        if (currentTile.tile_up)
                        {
                            currentTile = currentTile.tile_up;
                            transform.position = currentTile.transform.position;
                        }
                        else
                        {
                            Die();
                        }
                        break;
                    case direction.down:
                        if (currentTile.tile_down)
                        {
                            currentTile = currentTile.tile_down;
                            transform.position = currentTile.transform.position;
                        }
                        else
                        {
                            Die();
                        }
                        break;
                    case direction.left:
                        if (currentTile.tile_left)
                        {
                            currentTile = currentTile.tile_left;
                            transform.position = currentTile.transform.position;
                        }
                        else
                        {
                            Die();
                        }
                        break;
                    case direction.right:
                        if (currentTile.tile_right)
                        {
                            currentTile = currentTile.tile_right;
                            transform.position = currentTile.transform.position;
                        }
                        else
                        {
                            Die();
                        }
                        break;
                }

                timeSinceLastMove = 0;


            }
            else
            {
                Debug.LogError("No current Tile");
                return;
            }
            


           


            

            
        }
    }

    public void ChangeDirection(direction newDirection)
    {
        //set current direction to desired direction
        currentMovingDirection = newDirection;
        Debug.Log("changed direction");
    }
    

    
}
