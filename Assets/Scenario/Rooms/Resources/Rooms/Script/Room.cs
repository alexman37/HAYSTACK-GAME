using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Room
{
    public static Map mapReference;

    //How many pixels is each tile?
    public static int tileDelta = 60;

    public int id;
    public string roomName;
    public int roomType; //the types vary based on which template it's a part of
    public List<Room> adjacentRooms;
    public List<RoomBoundBox> boundaries;

    //used in "get random position in the room" calculations.
    private List<int> boundaryProbCutoffs = new List<int>();
    private int roomSize = 0;

    public List<RoomObject> objects = new List<RoomObject>();
    private HashSet<Vector2Int> locationsWithAnObject = new HashSet<Vector2Int>();

    //Used in camera logic.
    public Vector3 focalPoint;

    //Used when deleting unused objects
    private GameObject usedObjects = new GameObject();

    public Room(int id, string n, int t, List<Room> adj, List<RoomBoundBox> RoomBoundBoxes, int floorTile)
    {
        this.id = id;
        this.roomName = n;
        this.roomType = t;
        this.adjacentRooms = adj;
        this.boundaries = RoomBoundBoxes;

        Vector2 minBounds = boundaries[0].bottomLeft;
        Vector2 maxBounds = boundaries[0].topRight;

        foreach(RoomBoundBox bb in boundaries)
        {
            boundaryProbCutoffs.Add(roomSize + bb.size);
            roomSize += bb.size;

            if (bb.bottomLeft.x < minBounds.x) minBounds.x = bb.bottomLeft.x;
            if (bb.bottomLeft.y < minBounds.y) minBounds.y = bb.bottomLeft.y;
            if (bb.topRight.x > maxBounds.x) maxBounds.x = bb.bottomLeft.x;
            if (bb.topRight.y > maxBounds.y) maxBounds.y = bb.bottomLeft.y;
        }

        focalPoint = new Vector3(
            minBounds.x + ((maxBounds.x - minBounds.x) / 2),
            minBounds.y + ((maxBounds.y - minBounds.y) / 2),
            0 //TODO: Figure out a way to zoom out to the right amount
        );
    }


    //You are in this room, so draw everything
    public void draw()
    {
        foreach(RoomBoundBox bb in boundaries)
        {
            for(int x = bb.bottomLeft.x; x < bb.topRight.x; x++)
            {
                for (int y = bb.bottomLeft.y; y < bb.topRight.y; y++)
                {
                    RoomGen.tilemap.SetTile(new Vector3Int(x, y, 0), mapReference.template.allTiles[bb.tile]);
                }
            }
        }

        foreach(RoomObject obj in objects)
        {
            obj.activate();

            if (obj is RoomObjDoor door)
            {
                RoomGen.tilemap.SetTile(new Vector3Int(door.pos.x, door.pos.y, 0), mapReference.template.allTiles[door.doorTile]);
            }

            else if (obj is RoomObjCharacter ch)
            {
                GameObject g = GameObject.Instantiate(RoomGen.defaultSpr, new Vector3(ch.pos.x - 0.5f, ch.pos.y - 0.5f, 0), Quaternion.identity);
                g.transform.parent = usedObjects.transform;
                g.GetComponent<SpriteRenderer>().sprite = ch.sprite;
                ch.physicalCounterpart = g;
            }
        }
    }

    //You are not in this room, draw the floors in monochrome and skip all the objects
    public void drawOutline()
    {
        foreach (RoomBoundBox bb in boundaries)
        {
            for (int x = bb.bottomLeft.x; x < bb.topRight.x; x++)
            {
                for (int y = bb.bottomLeft.y; y < bb.topRight.y; y++)
                {
                    //Draw in monochrome
                    RoomGen.tilemap.SetTile(new Vector3Int(x, y, 0), mapReference.template.allTilesM[bb.tile]);
                }
            }
        }

        foreach (RoomObject obj in objects)
        {
            if (obj is RoomObjDoor door)
            {
                RoomGen.tilemap.SetTile(new Vector3Int(door.pos.x, door.pos.y, 0), mapReference.template.allTilesM[door.doorTile]);
            }
        }
    }

    //You have just left a room and want to remove the PHYSICAL objects from view.
    public void cleanupObjects()
    {
        foreach(RoomObject obj in objects)
        {
            obj.deactivate();
            obj.physicalCounterpart = null;
        }
        GameObject.Destroy(usedObjects);
        usedObjects = new GameObject();
    }

    //Is this an open tile in the room?
    public bool canMoveHere(Vector2Int dest)
    {
        foreach(RoomBoundBox bb in boundaries)
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

    public RoomObject isObjectHereExact(Vector2 dest)
    {
        foreach (RoomObject obj in objects)
        {
            if(obj.bbox.contains(dest))
            {
                return obj;
            }
        }

        return null;
    }

    public override string ToString()
    {
        return "ROOM # " + id + " - " + roomName;
    }

    public override bool Equals(object obj)
    {
        if (obj is Room) return (obj as Room).id == this.id;
        else return false;
    }

    //used when randomly choosing locations for room objects
    public Vector2Int getRandomPosition()
    {
        RoomBoundBox box = boundaries[indexInBPC(Random.Range(0, roomSize))];
        Vector2Int vec;
        int attempts = 0;
        do
        {
            vec = box.getRandomPosition();
            attempts++;
        } while (attempts < 16 && locationsWithAnObject.Contains(vec));

        if (attempts >= 16) Debug.LogError("Too many attempts for picking a random location. TODO: Handle this.");
        return vec;
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


public class PinpointBoundBox
{
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public float size;

    public PinpointBoundBox(Vector2 bl, Vector2 tr)
    {
        bottomLeft = bl;
        topRight = tr;

        size = (tr.x - bl.x) * (tr.y - bl.y);
    }

    public override string ToString()
    {
        return "[" + bottomLeft + " -- " + topRight + "]";
    }

    public bool contains(Vector2 dest)
    {
        return dest.x >= bottomLeft.x && dest.x < topRight.x && dest.y >= bottomLeft.y && dest.y < topRight.y;
    }
}

public class BoundBox
{
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public int size;

    public BoundBox(Vector2Int bl, Vector2Int tr)
    {
        bottomLeft = bl;
        topRight = tr;

        size = (tr.x - bl.x) * (tr.y - bl.y);
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


//This defines one of the boundaries of the room. All rooms can be defined by a list of rectangular boundaries
public class RoomBoundBox : BoundBox
{
    public bool walkable = true;
    public int tile;

    public RoomBoundBox(Vector2Int bl, Vector2Int tr, int t) : base(bl, tr)
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
}