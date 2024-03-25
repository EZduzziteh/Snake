using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enum_Direction;

public class Snake : MonoBehaviour
{
    List<Snake_Body> snakeBody = new List<Snake_Body>();
    [SerializeField]
    float timeBetweenMoves = 1.0f;
   
    direction currentMovingDirection = direction.down;
    float timeSinceLastMove = 0.0f;

    bool CanMove = true;

    Tile currentTile;
    bool isAlive = false;

    [SerializeField]
    Snake_Body snakeBodyPrefab;

    [SerializeField]
    Color snakeColor = Color.blue;

    public bool moveInProgress = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            if (CanMove)
            {
                StartCoroutine(HandleMove());
            }
           /* timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove >= timeBetweenMoves)
            {
                Move();
            }*/
        }
    }

    IEnumerator HandleMove()
    {
        CanMove = false;
        yield return new WaitForSeconds(timeBetweenMoves);
        Move();
        CanMove = true;

    }


    public void Grow()
    {
        Debug.Log("growing");
        Snake_Body newBody = GameObject.Instantiate(snakeBodyPrefab);
        
        //set new body part to the location we are currently at
        snakeBody.Add(newBody);
        newBody.transform.position = transform.position;
        newBody.currentTile = currentTile;
        //create new snake body
        //add to snake body list

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

            currentTile = FindAnyObjectByType<Locations>().GetStartTile();
            transform.position = currentTile.transform.position;
            currentTile.isOccupied = true;
            Debug.Log("start");
            isAlive = true;


            // move all foods to different tiles
            foreach(var f in FindObjectsOfType<Food>())
            {
                f.MoveToNewTile();
            }

            

        }
    }

    public void Move()
    {
       
        if (moveInProgress)
        {
            return;
        }
        moveInProgress = true;
        if (isAlive) { 
            if (currentTile)
            {
                switch (currentMovingDirection)
                {
                    case direction.up:
                        if (currentTile.tile_up)
                        {
                            HandleMovement(currentTile.tile_up);
                        }
                        else
                        {
                            Die();
                        }
                        break;
                    case direction.down:
                        if (currentTile.tile_down)
                        {
                            HandleMovement(currentTile.tile_down);
                        }
                        else //if no tile, die
                        {
                            Die();
                        }
                        break;
                    case direction.left:
                        if (currentTile.tile_left)
                        {
                            HandleMovement(currentTile.tile_left);
                        }
                        else //if no tile, die
                        {
                            Die();
                        }
                        break;
                    case direction.right:
                        if (currentTile.tile_right)
                        {
                            HandleMovement(currentTile.tile_right);
                        }
                        else //if no tile, die
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

        
        moveInProgress = false;
    }

    private void HandleMovement(Tile newTile)
    {
      

        if (newTile.isOccupied)
        {
            Die();
        }
        else //free to move
        {
            //if food, grow
            if (newTile.CheckHasFood())
            {
                //free to move, consume food
                newTile.ClearFood();
                //become larger
                Grow();
            }


            //then move
            ////start at the rear: move to equal the position of the next,
            //when we reach the front, move based on current direction.
            //
            if (snakeBody.Count > 0)
            {

                for (int i = snakeBody.Count - 1; i >= 0; i--)
                {
                    //first element iwil follow snake head, so move that last. 
                    if (i == 0)
                    {
                        snakeBody[i].gameObject.transform.position = currentTile.transform.position;
                        snakeBody[i].currentTile = currentTile;
                    }
                    else
                    {
                        
                        
                        //if the tail, set current tile to unoccupied
                        if (i == snakeBody.Count - 1)
                        {
                            snakeBody[i].currentTile.isOccupied = false;
                        }
                        //get the snakebody ahead of it, and it move to that location to move like a "train"
                        snakeBody[i].transform.position = snakeBody[i - 1].transform.position;

                        //set current tile to the next body elements tile.
                        snakeBody[i].currentTile = snakeBody[i - 1].currentTile;

                    }
                }
            }
            else
            {
                //if we have no body, set the old tile to unoccupied first
                currentTile.isOccupied = false;
            }

            //then move snake head
            currentTile = newTile;
            transform.position = currentTile.transform.position;

        }
    }

    public void ChangeDirection(direction newDirection)
    {
        //set current direction to desired direction
        currentMovingDirection = newDirection;
        Debug.Log("changed direction");
    }
    

    
}
