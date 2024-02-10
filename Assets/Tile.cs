using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update

    public Tile tile_up;
    public Tile tile_down;
    public Tile tile_left;
    public Tile tile_right;

    public bool isOccupied = false;

  
    Tile GetAdjacentTile(Enum_Direction.direction direction)
    {
        switch (direction)
        {
            case Enum_Direction.direction.up:
                if (tile_up)
                {
                    return tile_up;
                }
                break;
            case Enum_Direction.direction.down:
                if (tile_down)
                {
                    return tile_down;
                }
                break;
            case Enum_Direction.direction.left:
                if (tile_left)
                {
                    return tile_left;
                }
                break;
            case Enum_Direction.direction.right:
                if (tile_right)
                {
                    return tile_right;
                }
                break;
        }


        return null;
    }
}
