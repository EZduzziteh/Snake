using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Snake controlledSnake;

    
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


}
