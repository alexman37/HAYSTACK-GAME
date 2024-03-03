using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map {
    public List<Room> rooms;
    public static event Action generationCompleted;
    private Roster roster;


    //TODO: Generate using a template
    public void initializeMap(Roster rost)
    {
        roster = rost;

        rooms = RoomGen_Train.generate(8, rost);

        foreach (Room r in rooms)
        {
            r.draw();
        }
        
        generationCompleted.Invoke();
        //When map generation done, notify the player
    }
}
