using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Room
{
    //How many pixels is each tile?
    public static int tileDelta = 60;

    public int id;
    public string roomName;
    public int roomType; //the types vary based on which template it's a part of
    public List<Room> adjacentRooms;
    public List<BoundBox> boundaries;

    public List<RoomObject> objects = new List<RoomObject>();

    public Room(int id, string n, int t, List<Room> adj, List<BoundBox> boundBoxes, int floorTile)
    {
        this.id = id;
        this.roomName = n;
        this.roomType = t;
        this.adjacentRooms = adj;
        this.boundaries = boundBoxes;
    }

    public void draw()
    {
        foreach(BoundBox bb in boundaries)
        {
            for(int x = bb.bottomLeft.x; x < bb.topRight.x; x++)
            {
                for (int y = bb.bottomLeft.y; y < bb.topRight.y; y++)
                {
                    RoomGen.tilemap.SetTile(new Vector3Int(x, y, 0), bb.tile);
                }
            }
        }

        foreach(RoomObject obj in objects)
        {
            if(obj is RoomObjDoor door)
            {
                RoomGen.tilemap.SetTile(new Vector3Int(door.pos.x, door.pos.y, 0), door.doorTile);
            }
        }
    }

    //Is this an open tile in the room?
    public bool canMoveHere(Vector2 dest)
    {
        
        foreach(BoundBox bb in boundaries)
        {
            if (bb.walkable && bb.contains(dest))
            {
                //TODO: look for objects at this location
                return true;
            }
        }

        return false;
    }

    public RoomObject isObjectHere(Vector2Int dest)
    {
        foreach (RoomObject obj in objects)
        {
            if (obj.pos == dest)
            {
                //TODO: look for objects at this location
                return obj;
            }
        }
        return null;
    }

    public override string ToString()
    {
        return "ROOM # " + id + " - " + roomName;
    }
}


//This defines one of the boundaries of the room. All rooms can be defined by a list of rectangular boundaries
public class BoundBox
{
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public bool walkable = true;
    public TileBase tile;

    public BoundBox(Vector2Int bl, Vector2Int tr, TileBase t)
    {
        bottomLeft = bl;
        topRight = tr;
        tile = t;
    }

    public override string ToString()
    {
        return "[" + bottomLeft + " -- " + topRight + "]";
    }

    public bool contains(Vector2 dest)
    {
        return dest.x > bottomLeft.x && dest.x < topRight.x && dest.y > bottomLeft.y && dest.y < topRight.y;
    }
}