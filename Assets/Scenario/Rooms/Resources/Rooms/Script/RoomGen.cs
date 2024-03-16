using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class RoomGen : MonoBehaviour
{
    public static Tilemap tilemap;
    public static Map map;
    public static GameObject defaultSpr;


    public static event Action roomGenReady;

    // Start is called before the first frame update
    void Start()
    {
        roomGenReady += dummyMethod;
        defaultSpr = GameObject.FindGameObjectWithTag("DefaultSpr");
        tilemap = GameObject.FindGameObjectWithTag("TilemapSt").GetComponent<Tilemap>();

        Scenario.rosterCreationDone += initMap;
        roomGenReady.Invoke();
    }

    void initMap(Roster r)
    {
        Debug.Log("init map");
        map = new Map(AllTemplates.templateList[0]);
        map.initializeMap(r);
    }

    void dummyMethod() { }
}
