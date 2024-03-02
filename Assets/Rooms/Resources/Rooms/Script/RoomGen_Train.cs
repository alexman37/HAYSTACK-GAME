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
    static TileBase[] allTiles;

    //Acquire all tiles from the train tilemap
    static private void getAllTiles()
    {
        DirectoryInfo d = new DirectoryInfo("Assets/Rooms/Resources/Rooms/Tiles/Palettes/Train/Tiles");
        FileInfo[] info = d.GetFiles();

        allTiles = new TileBase[info.Length];

        for (int i = 0; i < info.Length; i++)
        {
            allTiles[i] = Resources.Load<TileBase>("Rooms/Tiles/Palettes/Train/Tiles/tileMap_" + i);
        }
    }

    //generate a single train car
    public static Room generateCar(int id, Vector2Int bottomLeft, int width, int height)
    {
        //STEP A: set up bounding boxes.
        bottomLeft = new Vector2Int(bottomLeft.x, bottomLeft.y);
        Vector2Int topRight = new Vector2Int(bottomLeft.x + width, bottomLeft.y + height);
        BoundBox b = new BoundBox(bottomLeft, topRight, allTiles[0]);
        List<BoundBox> boundBoxes = new List<BoundBox>();
        boundBoxes.Add(b);

        //STEP B: Set room properties.
        Room newRoom = new Room(id, "Train Car", 0, new List<Room>(), boundBoxes, trainFloorTile);

        return newRoom;

        //draw the room in RoomGen
        //assign doors at the end of generate()
    }

    public static List<Room> generate(int numCars)
    {
        getAllTiles();

        List<Room> rooms = new List<Room>();

        //Keep track of where doors will go.
        List<Vector2Int> doorLocs = new List<Vector2Int>();

        for(int i = 0; i < numCars; i++)
        {
            rooms.Add(generateCar(i, new Vector2Int(-i * 13, 0), 12, 5));
            if (i < numCars - 1) doorLocs.Add(new Vector2Int(-i * 13 - 1, 2));
        }

        //do adjacencies at the end once all rooms are created
        for (int i = 0; i < numCars; i++)
        {
            if (i > 0) rooms[i].adjacentRooms.Add(rooms[i - 1]);
            if (i < numCars - 1)
            {
                rooms[i].adjacentRooms.Add(rooms[i + 1]);
                //TODO: DOORS
                RoomObjDoor d = new RoomObjDoor(doorLocs[i], allTiles[10], (rooms[i], rooms[i+1]), (new Vector2(-i * 13 + 0.5f, 2.5f), new Vector2(-i * 13 - 1.5f, 2.5f)));
                rooms[i].objects.Add(d);
                rooms[i+1].objects.Add(d);
            }
        }

        GameObject g = GameObject.Instantiate(RoomGen.defaultSpr, Vector3.zero, Quaternion.identity);
        g.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Rooms/Objects/test_crate");

        return rooms;
    }
}
