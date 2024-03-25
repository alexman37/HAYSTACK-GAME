using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//There will only ever be one active at a time...BUT we will avoid making it "static" so we can save/load them I guess
public class Map {
    public List<Room> rooms;
    public static event Action<Room> generationCompleted;
    private Roster roster;
    public Template template;
    

    public Map(Template t)
    {
        this.template = t;
        Room.mapReference = this;
    }

    public void initializeMap(Roster rost)
    {
        roster = rost;

        rooms = RoomGen_Train.generate(8, rost);

        //TODO: Draw the starting / victim room first
        foreach (Room r in rooms)
        {
            if (r.id == 0) r.draw();
            else r.drawOutline();
        }

        Room.mapReference = this;

        generationCompleted.Invoke(rooms[0]);
        //When map generation done, notify the player
    }

}
