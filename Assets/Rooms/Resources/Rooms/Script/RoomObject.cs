using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public abstract class RoomObject {
    public Vector2Int pos;

    public virtual void interact()
    {
        Debug.Log("No implementation given for this interactable object.");
    }

    public RoomObject(Vector2Int pos)
    {
        this.pos = pos;
    }
}

public class RoomObjCharacter : RoomObject
{
    Character ch;
    public Sprite sprite;

    public RoomObjCharacter(Character ch, Sprite spr, Vector2Int pos) : base(pos)
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

public class RoomObjItem : RoomObject
{
    public RoomObjItem(Vector2Int pos) : base(pos)
    {

    }
}

public class RoomObjDoor : RoomObject
{
    public static event Action<Room> doorOpened;

    public TileBase doorTile;
    public (Room r1, Room r2) rooms;
    public (Vector2Int pos1, Vector2Int pos2) positions;

    public Vector2Int openDoor(Room currRoom)
    {
        Debug.Log("Going into another room from " + currRoom);
        if (currRoom.id == rooms.r1.id)
        {
            doorOpened.Invoke(rooms.r2);
            return positions.pos2;
        }
        else
        {
            doorOpened.Invoke(rooms.r1);
            return positions.pos1;
        }
    }

    public override void interact()
    {
        Debug.Log("Opened a door");
    }

    public RoomObjDoor(Vector2Int pos, TileBase tile, (Room r1, Room r2) rooms, (Vector2Int pos1, Vector2Int pos2) positions) : base(pos)
    {
        doorTile = tile;
        this.rooms = rooms;
        this.positions = positions;
    }
} 