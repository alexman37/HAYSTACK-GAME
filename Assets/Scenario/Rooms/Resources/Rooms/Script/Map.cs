using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map {
    public List<Room> rooms;
    public static event Action<Room> generationCompleted;
    private Roster roster;
    public Template template;
    

    public Map(Template t)
    {
        this.template = t;
    }

    public void initializeMap(Roster rost)
    {
        roster = rost;

        rooms = RoomGen_Train.generate(8, rost);

        //TODO: Only draw the current room, "Draw" other ones
        foreach (Room r in rooms)
        {
            r.draw();
        }
        
        generationCompleted.Invoke(rooms[0]);
        //When map generation done, notify the player
    }

}
