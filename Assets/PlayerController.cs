using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Snake controlledSnake;

    public GameObject GameOverPanel;
    


    // Start is called before the first frame update
    void Start()
    {
        controlledSnake = GetComponent<Snake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            
            controlledSnake.Respawn();
            GameOverPanel.SetActive(false);
          
            
        }

        if (controlledSnake) {
            if (Input.GetKeyDown(KeyCode.W))
            {
                controlledSnake.ChangeDirection(Enum_Direction.direction.up);

            } else if (Input.GetKeyDown(KeyCode.S))
            {
                controlledSnake.ChangeDirection(Enum_Direction.direction.down);

            } else if (Input.GetKeyDown(KeyCode.A))
            {
                controlledSnake.ChangeDirection(Enum_Direction.direction.left);

            } else if (Input.GetKeyDown(KeyCode.D))
            {
                controlledSnake.ChangeDirection(Enum_Direction.direction.right);
            }  
        }

    }

    internal void Die()
    {
        Debug.Log("dead");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
