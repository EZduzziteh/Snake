using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Snake controlledSnake;

    public GameObject GameOverPanel;

    Tile nextTile;




    // Start is called before the first frame update
    void Start()
    {
        controlledSnake = GetComponent<Snake>();
    }

    // Update is called once per frame
    void Update()
    {

        if(controlledSnake.isAlive == false)
        {
            if (Input.anyKeyDown)
            {
                if (controlledSnake.isAiControlled)
                {
                    controlledSnake.StartAI();
                }
                else
                {
                    controlledSnake.StartPlayer();
                }

                foreach(var snake in FindObjectsOfType<Snake>())
                {
                    if (snake.isAiControlled)
                    {
                        snake.StartAI();
                    }
                }

                //move all foods to different tiles
                foreach (var f in FindObjectsOfType<Food>())
                {
                    f.MoveToNewTile();
                }

                GameOverPanel.SetActive(false);
            }
        }
        if (!controlledSnake.isAiControlled)
        {

            if (controlledSnake)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    //disallow 180 degree movement
                    if (controlledSnake.GetCurrentDirection() != Enum_Direction.direction.down)
                    {
                        controlledSnake.ChangeDirection(Enum_Direction.direction.up);
                    }

                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    //disallow 180 degree movement
                    if (controlledSnake.GetCurrentDirection() != Enum_Direction.direction.up)
                    {
                        controlledSnake.ChangeDirection(Enum_Direction.direction.down);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    //disallow 180 degree movement
                    if (controlledSnake.GetCurrentDirection() != Enum_Direction.direction.right)
                    {
                        controlledSnake.ChangeDirection(Enum_Direction.direction.left);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    //disallow 180 degree movement
                    if (controlledSnake.GetCurrentDirection() != Enum_Direction.direction.left)
                    {
                        controlledSnake.ChangeDirection(Enum_Direction.direction.right);
                    }
                }
            }
        }

    }

    internal void Die()
    {
        Debug.Log("dead");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
