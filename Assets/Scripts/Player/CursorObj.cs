using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//CursorObj's goal is to recognize when it is hovering over / clicking on something:
// - A UI object
// - A Physical room object in the world
public class CursorObj : MonoBehaviour
{
    Room currentRoom;

    private void Start()
    {
        Map.generationCompleted += essentiallyInitializePlayer;
        RoomObjDoor.doorOpened += moveToNewRoom;
    }

    void essentiallyInitializePlayer(Room start)
    {
        moveToNewRoom(start);
    }

    void moveToNewRoom(Room dest)
    {
        currentRoom = dest;
        this.gameObject.transform.position = currentRoom.focalPoint;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RoomObject obj = currentRoom.isObjectHereExact(mouseToGrid());
            if (obj != null) obj.interact();
        }
    }

    private Vector2 mouseToGrid()
    {
        Vector2 vec = new Vector2Int();
        vec.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        vec.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        return vec;
    }
}
