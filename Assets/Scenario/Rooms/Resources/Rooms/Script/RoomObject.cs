using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


//A "room object" is anything that can be clicked on on the map
public abstract class RoomObject {
    public Vector2Int pos;
    public PinpointBoundBox bbox;
    public bool active = false;

    public virtual void interact()
    {
        if(active) Debug.Log("No implementation given for this interactable object.");
    }

    public RoomObject(Vector2Int pos, PinpointBoundBox bbox)
    {
        this.pos = pos;
        this.bbox = bbox;
    }

    public void activate() { active = true; }

    public void deactivate() { active = false; }
}

//CHARACTERS
public class RoomObjCharacter : RoomObject
{
    Character ch;
    public Sprite sprite;

    public RoomObjCharacter(Character ch, Sprite spr, Vector2Int pos, PinpointBoundBox bbox) : base(pos, bbox)
    {
        this.ch = ch;
        this.pos = pos;
        sprite = spr;
    }

    public override void interact()
    {
        if(active)
        {
            Debug.Log("Talking to " + ch.getDisplayName(false));
        }
    }
}


//ITEMS
public class RoomObjItem : RoomObject
{
    public RoomObjItem(Vector2Int pos, PinpointBoundBox bbox) : base(pos, bbox)
    {

    }
}


//DOORS
public class RoomObjDoor : RoomObject
{
    public static event Action<Room> doorOpened;

    public int doorTile;
    public Room destinationRoom;

    public override void interact()
    {
        if (active)
        {
            Debug.Log("Opened a door");
            doorOpened.Invoke(destinationRoom);
        }
        else Debug.Log("Refused to open door as it is inactive.");
    }

    public RoomObjDoor(Vector2Int pos, int tile, Room room, PinpointBoundBox bbox) : base(pos, bbox)
    {
        doorTile = tile;
        this.destinationRoom = room;
    }
} 