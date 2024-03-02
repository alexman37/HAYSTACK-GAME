using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map {
    public List<Room> rooms;
    public static event Action generationCompleted;

    //TODO: Generate using a template
    public void initializeMap()
    {
        rooms = RoomGen_Train.generate(8);

        foreach (Room r in rooms)
        {
            r.draw();
        }
        
        generationCompleted.Invoke();
        //When map generation done, notify the player
    }
}
