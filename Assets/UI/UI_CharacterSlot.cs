using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_CharacterSlot : MonoBehaviour
{
    public static event Action<AgentCharacter> characterSelect;

    //Probably not actually used, just here for reference
    public static AgentCharacter[] allCharacters = { 
        new AgentCharacter("Hazel", new Color(16f / 255f, 122f / 255f, 28f / 255f), true, false),
        new AgentCharacter("Winter", new Color(16f / 255f, 122f / 255f, 140f / 255f), false, true)
    };

    public int whichCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectThisCharacter()
    {
        Debug.Log("I've selected " + allCharacters[whichCharacter]);
        characterSelect.Invoke(allCharacters[whichCharacter]);
    }
}

//Hazel, Winter, etc.
public class AgentCharacter
{
    public string name;
    public Color color;
    public bool talks;
    public bool thinks;

    public AgentCharacter(string n, Color c, bool talks, bool thinks)
    {
        name = n;
        color = c;
        this.talks = talks;
        this.thinks = thinks;
    }

    public override string ToString()
    {
        return name;
    }
}