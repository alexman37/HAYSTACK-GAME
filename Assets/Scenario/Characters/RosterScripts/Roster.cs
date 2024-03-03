using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Facilitates generation, storage and access of the list of 100 suspects (characters 1-100), and the murder victim (character 0).
public class Roster
{
    public List<Character> roster;
    public List<Sprite> rosterSprites; //consistent list of the portrait per each character

    public Roster(int numChars, Template temp)
    {
        createRoster(numChars, temp);
    }

    public void createRoster(int numChars, Template temp)
    {
        if (roster != null)
        {
            roster.Clear();
            rosterSprites.Clear();
        } else
        {
            roster = new List<Character>();
            rosterSprites = new List<Sprite>();
        }

        for(int i = 0; i <= numChars; i++)
        {
            roster.Add(new Character(
                i //the id
            ));

            roster[i].randomizeDemographics();

            rosterSprites.Add(CharSpriteGen.genSpriteFromLayers(roster[i]));
        }
    }

    public void DebugLogRoster()
    {
        for (int i = 0; i < roster.Count; i++)
        {
            Debug.Log(roster[i]);
        }
    }
}
