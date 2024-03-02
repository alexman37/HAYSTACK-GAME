using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGen : MonoBehaviour
{
    public static Tilemap tilemap;
    public static Map map;
    public static GameObject defaultSpr;

    // Start is called before the first frame update
    void Start()
    {
        defaultSpr = GameObject.FindGameObjectWithTag("DefaultSpr");
        tilemap = GameObject.FindGameObjectWithTag("TilemapSt").GetComponent<Tilemap>();

        map = new Map();
        map.initializeMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
