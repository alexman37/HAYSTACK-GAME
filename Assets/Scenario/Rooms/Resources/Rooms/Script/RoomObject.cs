using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


//A "room object" is anything that can be clicked on on the map
public abstract class RoomObject {
    public Vector2Int pos;
    public PinpointBoundBox bbox;

    public virtual void interact()
    {
        Debug.Log("No implementation given for this interactable object.");
    }

    public RoomObject(Vector2Int pos, PinpointBoundBox bbox)
    {
        this.pos = pos;
        this.bbox = bbox;
    }
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
        Debug.Log("Talking to " + ch.getDisplayName(false));
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

    public TileBase doorTile;
    public Room destinationRoom;

    public override void interact()
    {
        Debug.Log("Opened a door");
        doorOpened.Invoke(destinationRoom);
    }

    public RoomObjDoor(Vector2Int pos, TileBase tile, Room room, PinpointBoundBox bbox) : base(pos, bbox)
    {
        doorTile = tile;
        this.destinationRoom = room;
    }
} 