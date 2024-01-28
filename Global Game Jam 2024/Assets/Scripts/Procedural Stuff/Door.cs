using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : Item
{
    private static BoardManager boardManager;
    string destinationScene = "new room";

    public enum Direction
    {
        left, right, up, down
    }

    public Direction doorDirection;

    private void Awake()
    {
        if (boardManager == null)
        {
            boardManager = Object.FindObjectOfType<BoardManager>();
        }
    }


    // Start is called before the first frame update
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (destinationScene == "new room")
            {
                
            }
        }
    }*/

    protected override void Interact()
    {
        if ((doorDirection == Direction.up && GameManager.topDoorLocked == false) || (doorDirection == Direction.down && GameManager.bottomDoorLocked == false) || (doorDirection == Direction.left && GameManager.leftDoorLocked == false) || (doorDirection == Direction.right && GameManager.bottomDoorLocked == false))
        {
            boardManager.MoveToNewRoom(doorDirection);
        }
    }
}