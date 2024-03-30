using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.IO;


/*TODOS (general for map generation)
 
 - A better system for setting up doors (what happens when rooms get complicated?)*/
public static class RoomGen_Train
{
    static int trainFloorTile = 0;
    static Template trainTemp = AllTemplates.templateList[0];

    //generate a single train car
    public static Room generateCar(int id, Vector2Int bottomLeft, int width, int height)
    {
        //STEP A: set up bounding boxes.
        bottomLeft = new Vector2Int(bottomLeft.x, bottomLeft.y);
        Vector2Int topRight = new Vector2Int(bottomLeft.x + width, bottomLeft.y + height);
        RoomBoundBox b = new RoomBoundBox(bottomLeft, topRight, 0);
        List<RoomBoundBox> RoomBoundBoxes = new List<RoomBoundBox>();
        RoomBoundBoxes.Add(b);

        //STEP B: Set room properties.
        Room newRoom = new Room(id, "Train Car", 0, new List<Room>(), RoomBoundBoxes, trainFloorTile);

        //STEP C: Add objects to room.

        return newRoom;

        //draw the room in RoomGen
        //assign doors at the end of generate()
    }

    public static List<Room> generate(int numCars, Roster rost)
    {
        List<Room> rooms = new List<Room>();

        //Keep track of where doors will go.
        //List<Vector2Int> doorLocs = new List<Vector2Int>();

        for(int i = 0; i < numCars; i++)
        {
            rooms.Add(generateCar(i, new Vector2Int(-i * 13, 0), 12, 5));
            //if (i < numCars - 1) doorLocs.Add(new Vector2Int(-i * 13 - 1, 2));
        }

        //do adjacencies at the end once all rooms are created
        for (int i = 0; i < numCars; i++)
        {
            if (i > 0)
            {
                //ADD DOOR - RIGHT ROOM
                rooms[i].adjacentRooms.Add(rooms[i - 1]);
                PinpointBoundBox bb = new PinpointBoundBox(new Vector2((-i + 1) * 13 - 1 - 0.5f, 1.5f), new Vector2((-i + 1) * 13 - 1 + 0.5f, 2.5f));
                RoomObjDoor d = new RoomObjDoor(new Vector2Int((-i + 1) * 13 - 1, 2), 10, rooms[i - 1], bb);
                Debug.Log("Right door - " + d.bbox);
                rooms[i].objects.Add(d);
            }
            if (i < numCars - 1)
            {
                //ADD DOOR - LEFT ROOM
                rooms[i].adjacentRooms.Add(rooms[i + 1]);
                PinpointBoundBox bb = new PinpointBoundBox(new Vector2(-i * 13 - 1 - 0.5f, 1.5f), new Vector2(-i * 13 - 1 + 0.5f, 2.5f));
                RoomObjDoor d = new RoomObjDoor(new Vector2Int(-i * 13 - 1, 2), 10, rooms[i + 1], bb);
                rooms[i].objects.Add(d);
            }
        }

        //randomly assign objects
        rooms = randomlyAssignObjects(rooms, rost);

        //Example of spawning an item
        //GameObject g = GameObject.Instantiate(RoomGen.defaultSpr, Vector3.zero, Quaternion.identity);
        //g.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Rooms/Objects/test_crate");

        return rooms;
    }





    private static List<Room> randomlyAssignObjects(List<Room> rooms, Roster rost)
    {
        foreach(Character ch in rost.roster)
        {
            int roomIx = Random.Range(0, rooms.Count);
            Vector2Int pos = rooms[roomIx].getRandomPosition();
            PinpointBoundBox bb = new PinpointBoundBox(pos + new Vector2(-0.5f, -0.5f), pos + new Vector2(0.5f,0.5f));
            rooms[roomIx].addObject(new RoomObjCharacter(ch, pos, bb));
        }

        return rooms;
    }
}
