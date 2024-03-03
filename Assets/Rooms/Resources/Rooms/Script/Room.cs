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

    //used in "get random position in the room" calculations.
    private List<int> boundaryProbCutoffs = new List<int>();
    private int roomSize = 0;

    public List<RoomObject> objects = new List<RoomObject>();
    private HashSet<Vector2Int> locationsWithAnObject = new HashSet<Vector2Int>();

    public Room(int id, string n, int t, List<Room> adj, List<BoundBox> boundBoxes, int floorTile)
    {
        this.id = id;
        this.roomName = n;
        this.roomType = t;
        this.adjacentRooms = adj;
        this.boundaries = boundBoxes;

        foreach(BoundBox bb in boundaries)
        {
            boundaryProbCutoffs.Add(roomSize + bb.size);
            roomSize += bb.size;
        }
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

            else if (obj is RoomObjCharacter ch)
            {
                GameObject g = GameObject.Instantiate(RoomGen.defaultSpr, new Vector3(ch.pos.x, ch.pos.y, 0), Quaternion.identity);
                g.GetComponent<SpriteRenderer>().sprite = ch.sprite;
            }
        }
    }

    //Is this an open tile in the room?
    public bool canMoveHere(Vector2Int dest)
    {
        foreach(BoundBox bb in boundaries)
        {
            if (bb.walkable && bb.contains(dest))
            {
                if (isObjectHere(dest) != null) return false;
                else return true;
            }
        }

        return false;
    }

    public void addObject(RoomObject obj)
    {
        objects.Add(obj);
        locationsWithAnObject.Add(obj.pos);
    }

    public RoomObject isObjectHere(Vector2Int dest)
    {
        //small optimization
        if(locationsWithAnObject.Contains(dest)) {
            foreach (RoomObject obj in objects)
            {
                if (obj.pos == dest)
                {
                    return obj;
                }
            }
        }
        
        return null;
    }

    public override string ToString()
    {
        return "ROOM # " + id + " - " + roomName;
    }

    public Vector2Int getRandomPosition()
    {
        BoundBox box = boundaries[indexInBPC(Random.Range(0, roomSize))];
        return box.getRandomPosition();
    }

    private int indexInBPC(int num)
    {
        //TODO: Binary search would be faster...
        for(int i = 0; i < boundaryProbCutoffs.Count; i++)
        {
            if (num < boundaryProbCutoffs[i]) return i;
        }
        return boundaryProbCutoffs.Count - 1;
    }
}


//This defines one of the boundaries of the room. All rooms can be defined by a list of rectangular boundaries
public class BoundBox
{
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public int size;
    public bool walkable = true;
    public TileBase tile;

    public BoundBox(Vector2Int bl, Vector2Int tr, TileBase t)
    {
        bottomLeft = bl;
        topRight = tr;
        tile = t;

        size = (tr.x - bl.x) * (tr.y - bl.y);
    }

    public Vector2Int getRandomPosition()
    {
        return new Vector2Int(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y));
    }

    public override string ToString()
    {
        return "[" + bottomLeft + " -- " + topRight + "]";
    }

    public bool contains(Vector2Int dest)
    {
        return dest.x >= bottomLeft.x && dest.x < topRight.x && dest.y >= bottomLeft.y && dest.y < topRight.y;
    }
}