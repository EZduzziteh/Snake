using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enum_Direction;

public class Snake : MonoBehaviour
{
    public
    List<Snake_Body> snakeBody = new List<Snake_Body>();
    [SerializeField]
    float timeBetweenMoves = 1.0f;
   
    direction currentMovingDirection = direction.down;
    direction desiredDirection = direction.down;
    float timeSinceLastMove = 0.0f;

    bool CanMove = true;

    Tile currentTile;
    bool isAlive = false;

    [SerializeField]
    Snake_Body snakeBodyPrefab;

    [SerializeField]
    Color snakeColor = Color.blue;


    PlayerController controller;


    public bool moveInProgress = false;


    public void OccupyAllTiles()
    {
        currentTile.isOccupied = true;
        foreach(var body in snakeBody)
        {
            body.currentTile.isOccupied = true;
        }
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

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
        if (controller)
        {
            controller.Die();
        }
    }

    public void Respawn()
    {
        if (!isAlive)
        {
            if (currentTile)
            {
                currentTile.isOccupied = false;
                Debug.Log(currentTile.name + " unoccupied");
            }

            currentTile = FindAnyObjectByType<Locations>().GetStartTile();
            transform.position = currentTile.transform.position;
            currentTile.isOccupied = true;
            Debug.Log(currentTile.name + " ccupied");
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
        currentMovingDirection = desiredDirection;
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
        //this is here so that we dont update direction in the middle of a move.
        

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
                    Debug.Log("moving body: "+ i+ " for :"+snakeBody[i].name);
                    //first element iwil follow snake head, so move that last. 
                    if (i == 0)
                    {
                        snakeBody[i].gameObject.transform.position = currentTile.transform.position;
                        snakeBody[i].currentTile = currentTile;
                        snakeBody[i].currentTile.isOccupied = true;
                        Debug.Log(currentTile.name + " occupied");

                    }
                    else if (i== snakeBody.Count - 1)
                    {
                        //if we are the tail, release our tile from being occupied.
                        snakeBody[i].currentTile.isOccupied = false;
                        Debug.Log(currentTile.name + " unoccupied");
                        //get the snakebody ahead of it, and it move to that location to move like a "train"
                        snakeBody[i].transform.position = snakeBody[i - 1].transform.position;

                        //set current tile to the next body elements tile.
                        snakeBody[i].currentTile = snakeBody[i - 1].currentTile;

                        //set current tile ot occupied
                        snakeBody[i].currentTile.isOccupied = true;
                        Debug.Log(currentTile.name + " occupied");
                    }
                    else
                    {
                       
                        //get the snakebody ahead of it, and it move to that location to move like a "train"
                        snakeBody[i].transform.position = snakeBody[i - 1].transform.position;

                        //set current tile to the next body elements tile.
                        snakeBody[i].currentTile = snakeBody[i - 1].currentTile;

                        //set current tile ot occupied
                        snakeBody[i].currentTile.isOccupied = true;
                        Debug.Log(currentTile.name + " occupied");

                    }
                }
            }
            else
            {
                //if we have no body, set the old tile to unoccupied first
                currentTile.isOccupied = false;
                Debug.Log(currentTile.name + " unoccupied");
            }

            //then move snake head
            currentTile = newTile;
            currentTile.isOccupied = true;
            transform.position = currentTile.transform.position;
            Debug.Log("moving head");
        }



        

    }

    public void ChangeDirection(direction newDirection)
    {
        //set current direction to desired direction
       // currentMovingDirection = newDirection;
        desiredDirection = newDirection;
        Debug.Log("changed direction");
    }
    

    
}
