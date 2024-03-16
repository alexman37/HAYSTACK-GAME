using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



//Scenario is the highest level "manager" in the game, it manages the map and roster
//from their creation to ordinary uses

//Saves/Loads create/load the scenario objects
public class Scenario : MonoBehaviour
{
    public Roster rost;
    public Map map;

    //In relation to other events
    private bool roomGenReady;
    private bool rosterUIReady;

    //Events
    public static event Action<Roster> rosterCreationDone;

    // TODO: MOve from start method to alternative method, using a template of your choice
    void Start()
    {
        rosterCreationDone += some;
        AllTemplates.defineTemplates();

        //Subscriptions
        RoomGen.roomGenReady += ReadyRoomGen;

        this.gameObject.AddComponent<RoomGen>();
    }

    //Call this only when all other necessary objects (RoomGen, RosterUI, etc.) are finished creation
    void createNewScenario()
    {
        //1. Generate a roster
        rost = new Roster(100, AllTemplates.templateList[0]);
        rost.DebugLogRoster();
        rosterCreationDone.Invoke(rost);

        //2. Generate the map
        map = new Map(AllTemplates.templateList[0]);

        //3. Generate sequence of events / timelines
    }

    void some(Roster r)
    {

    }


    void ReadyRoomGen()
    {
        roomGenReady = true;
        if (allSystemsReady()) createNewScenario();
    }

    bool allSystemsReady()
    {
        return roomGenReady;
    }
}
