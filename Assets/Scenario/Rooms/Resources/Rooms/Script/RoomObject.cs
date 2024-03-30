using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


//A "room object" is anything that can be clicked on on the map
public abstract class RoomObject {
    public Vector2Int pos;
    public PinpointBoundBox bbox;
    public bool active = false;
    public GameObject physicalCounterpart;

    public virtual void interact()
    {
        if(active) Debug.Log("No implementation given for this interactable object.");
    }

    public RoomObject(Vector2Int pos, PinpointBoundBox bbox)
    {
        this.pos = pos;
        this.bbox = bbox;
        physicalCounterpart = null;
    }

    public void activate() { active = true; }

    public void deactivate() { active = false; }

    public virtual void onCharacterSelect(AgentCharacter charac)
    {
        Debug.Log("A character was selected, but no action is implemented for this room object");
    }
}

//CHARACTERS
public class RoomObjCharacter : RoomObject
{
    Character ch;
    public Sprite sprite;

    //If no sprite is provided, generate one
    public RoomObjCharacter(Character ch, Vector2Int pos, PinpointBoundBox bbox) : base(pos, bbox)
    {
        this.ch = ch;
        this.pos = pos;
        sprite = PawnGen.generatePawnFromChar(ch);

        UI_CharacterSlot.characterSelect += onCharacterSelect;
    }

    public RoomObjCharacter(Character ch, Sprite spr, Vector2Int pos, PinpointBoundBox bbox) : base(pos, bbox)
    {
        this.ch = ch;
        this.pos = pos;
        sprite = spr;

        UI_CharacterSlot.characterSelect += onCharacterSelect;
    }

    public override void interact()
    {
        if(active)
        {
            Debug.Log("Talking to " + ch.getDisplayName(false));
        }
    }

    //Highlight a selected object
    public void highlight(Color col)
    {
        if(active)
        {
            physicalCounterpart.GetComponent<SpriteRenderer>().color = col;
            Debug.Log("Highlighting " + this);
        }
    }

    public void removeHighlight()
    {
        if(active)
        physicalCounterpart.GetComponent<SpriteRenderer>().color = Color.white;
    }

    //Detectives who talk can interact with characters
    public override void onCharacterSelect(AgentCharacter det)
    {
        removeHighlight();
        if(det.talks) highlight(det.color);
    }
}


//ITEMS
public class RoomObjItem : RoomObject
{
    public RoomObjItem(Vector2Int pos, PinpointBoundBox bbox) : base(pos, bbox)
    {

    }
}


//DOORS
public class RoomObjDoor : RoomObject
{
    public static event Action<Room> doorOpened;

    public int doorTile;
    public Room destinationRoom;

    public override void interact()
    {
        if (active)
        {
            Debug.Log("Opened a door");
            doorOpened.Invoke(destinationRoom);
        }
        else Debug.Log("Refused to open door as it is inactive.");
    }

    public RoomObjDoor(Vector2Int pos, int tile, Room room, PinpointBoundBox bbox) : base(pos, bbox)
    {
        doorTile = tile;
        this.destinationRoom = room;
    }
} 