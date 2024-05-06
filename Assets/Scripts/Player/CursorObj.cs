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

    //Clicked on a door, moving to new room
    void moveToNewRoom(Room dest)
    {
        //Cleanup old room
        if (currentRoom != null)
        {
            currentRoom.cleanupObjects();
            currentRoom.drawOutline();
        }

        //Move into new room
        currentRoom = dest;
        this.gameObject.transform.position = currentRoom.focalPoint;
        currentRoom.draw();
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
        Vector3 vec = new Vector3();
        vec = Input.mousePosition;
        vec.z = 10f;
        Debug.Log("True position: " + Input.mousePosition);
        vec = Camera.main.ScreenToWorldPoint(vec);
        //Debug.Log(Input.mousePosition);
        Debug.Log(vec);
        return new Vector2(vec.x, vec.y);
    }
}
