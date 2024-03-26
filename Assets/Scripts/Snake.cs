using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Enum_Direction;

public class Snake : MonoBehaviour
{

    //Set these in Editor
    [SerializeField]
    float timeBetweenMoves = 1.0f;

    [SerializeField]
    Snake_Body snakeBodyPrefab;

    public int aiDifficulty = 1;


    //Movement
    direction currentMovingDirection = direction.down;
    direction desiredDirection = direction.down;
    bool CanMove = true;
    Tile currentTile;
    public bool moveInProgress = false;

    //Status
    public bool isAlive = false;
    public bool isAiControlled = false;

    //References
    PlayerController controller;
    Locations location;

    public List<Snake_Body> snakeBody = new List<Snake_Body>();
    public List<Tile> path;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        location = FindObjectOfType<Locations>();
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
        }
    }

    public void OccupyAllTiles()
    {
        currentTile.isOccupied = true;
        foreach (var body in snakeBody)
        {
            body.currentTile.isOccupied = true;
        }
    }

    IEnumerator HandleMove()
    {
        CanMove = false;
        yield return new WaitForSeconds(timeBetweenMoves);
        if (isAiControlled)
        {
            AIMove();
        }
        else
        {
            Move();
        }
        CanMove = true;
        location.Fixup();
    }

    //handles movement logic for player 
    public void Move()
    {
        if (moveInProgress)
        {
            return;
        }
        currentMovingDirection = desiredDirection;
        moveInProgress = true;
        if (isAlive)
        {
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
            }
            else
            {
                Debug.LogError("No current Tile");
                return;
            }
        }
        moveInProgress = false;
    }
    //handles movement logic for AI
    private void AIMove()
    {
        if (moveInProgress)
        {
            return;
        }

        if (aiDifficulty == 3) { 
            GetNewAIPath();
        }
        

        //find out what direction to go for the next Djikstra path
        moveInProgress = true;

        
        if(currentTile.tile_down == path[0])
        {
            UnityEngine.Color hexColor;
            UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
            path[0].GetComponent<SpriteRenderer>().color = hexColor;
            path.Remove(path[0]);
            HandleMovement(currentTile.tile_down);
            moveInProgress = false;
            return;
        }
        else if(currentTile.tile_left == path[0])
        {
            UnityEngine.Color hexColor;
            UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
            path[0].GetComponent<SpriteRenderer>().color = hexColor;
            path.Remove(path[0]);
            HandleMovement(currentTile.tile_left);
            moveInProgress = false;
            return;
        }
        else if(currentTile.tile_right == path[0])
        {
            UnityEngine.Color hexColor;
            UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
            path[0].GetComponent<SpriteRenderer>().color = hexColor;
            path.Remove(path[0]);
            HandleMovement(currentTile.tile_right);
            moveInProgress = false;
            return;

        }
        else if(currentTile.tile_up == path[0])
        {
            UnityEngine.Color hexColor;
            UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
            path[0].GetComponent<SpriteRenderer>().color = hexColor;
            path.Remove(path[0]);
            HandleMovement(currentTile.tile_up);
            moveInProgress = false;
            return;
        }
        else
        {
            Debug.LogError("no current tile or no path");
            moveInProgress = false;
            return;
        }
    }


 
    
    //This is the step where movement actually gets executed
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
                    //Debug.Log("moving body: "+ i+ " for :"+snakeBody[i].name);
                    //first element will follow snake head, so move that last. 
                    if (i == 0)
                    {
                        snakeBody[i].gameObject.transform.position = currentTile.transform.position;
                        snakeBody[i].currentTile = currentTile;
                        snakeBody[i].currentTile.isOccupied = true;
                       // Debug.Log(currentTile.name + " occupied");
                    }
                    else if (i== snakeBody.Count - 1)
                    {
                        //if we are the tail, release our tile from being occupied.
                        snakeBody[i].currentTile.isOccupied = false;
                       //Debug.Log(currentTile.name + " unoccupied");
                        //get the snakebody ahead of it, and it move to that location to move like a "train"
                        snakeBody[i].transform.position = snakeBody[i - 1].transform.position;

                        //set current tile to the next body elements tile.
                        snakeBody[i].currentTile = snakeBody[i - 1].currentTile;

                        //set current tile ot occupied
                        snakeBody[i].currentTile.isOccupied = true;
                       // Debug.Log(currentTile.name + " occupied");
                    }
                    else
                    {
                       
                        //get the snakebody ahead of it, and it move to that location to move like a "train"
                        snakeBody[i].transform.position = snakeBody[i - 1].transform.position;

                        //set current tile to the next body elements tile.
                        snakeBody[i].currentTile = snakeBody[i - 1].currentTile;

                        //set current tile ot occupied
                        snakeBody[i].currentTile.isOccupied = true;
                       // Debug.Log(currentTile.name + " occupied");

                    }
                }
            }
            else
            {
                //if we have no body, set the old tile to unoccupied first
                currentTile.isOccupied = false;
                //Debug.Log(currentTile.name + " unoccupied");
            }

            //then move snake head
            currentTile = newTile;
            currentTile.isOccupied = true;
            transform.position = currentTile.transform.position;
           // Debug.Log("moving head");
        }


    }

    private void GetNewAIPath()
    {
        //first clear out the old path
        UnityEngine.Color hexColor;
        UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
        foreach (var tile in path)
        {
            //color first element as its been traversed
            tile.GetComponent<SpriteRenderer>().color = hexColor;
        }

        path.Clear();

        path = location.GetTilePathViaDjikstra(currentTile, FindObjectOfType<Food>().GetCurrentTile());

        //color first element as its been traversed

        path[0].GetComponent<SpriteRenderer>().color = hexColor;
        //remove first element as its where we started
        path.Remove(path[0]);

        foreach (var tile in path)
        {
            tile.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
        }
    }

    public void ChangeDirection(direction newDirection)
    {
        desiredDirection = newDirection;
    }

    public direction GetCurrentDirection()
    {
        return currentMovingDirection;
    }

    public void StartPlayer()
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
            isAlive = true;
        }
    }

    public void StartAI()
    {
        if (isAiControlled)
        {
            currentTile =  FindAnyObjectByType<Locations>().GetRandomUnoccupiedTile(); 
            transform.position = currentTile.transform.position;
            currentTile.isOccupied = true;
            isAlive = true;
            //generate initial path
            GetNewAIPath();
        }
    }

    //handle consuming food.
    public void Grow()
    {
       // Debug.Log("growing");
        Snake_Body newBody = GameObject.Instantiate(snakeBodyPrefab);
        //set new body part to the location we are currently at
        snakeBody.Add(newBody);
        newBody.transform.position = transform.position;
        newBody.currentTile = currentTile;
        newBody.currentTile.isOccupied = true;
        //create new snake body
        //add to snake body list
    }

    //handle death condition
    public void Die()
    {
        if (controller)
        {
            //#TODO reenable this
            controller.Die();
        }
        else
        {
            UnityEngine.Color hexColor;
            UnityEngine.ColorUtility.TryParseHtmlString("#785757", out hexColor);
            //unoccupy all tiles
            foreach (var body in snakeBody)
            {
                body.currentTile.isOccupied = false;
                body.currentTile.GetComponent<SpriteRenderer>().color = hexColor;
            }
            currentTile.isOccupied = false;
            currentTile.GetComponent<SpriteRenderer>().color = hexColor;
            Destroy(gameObject);
        }
    }



}
