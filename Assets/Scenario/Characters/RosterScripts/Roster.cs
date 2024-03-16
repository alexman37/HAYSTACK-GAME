using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Facilitates generation, storage and access of the list of 100 suspects (characters 1-100), and the murder victim (character 0).
public class Roster
{
    public int size;
    public List<Character> roster;
    public List<Sprite> rosterSprites; //consistent list of the portrait per each character

    public Roster(int numChars, Template temp)
    {
        size = numChars;
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

        List<(Role role, int amnt)> roleAmnts = determineRoleAmounts(temp);
        int roleIndex = 0;
        int amntCount = 0;

        for(int i = 0; i < numChars; i++)
        {
            roster.Add(new Character(
                i //the id
            ));

            roster[i].randomizeDemographics();

            rosterSprites.Add(CharSpriteGen.genSpriteFromLayers(roster[i]));

            roster[i].role = roleAmnts[roleIndex].role;
            amntCount++;
            if(amntCount >= roleAmnts[roleIndex].amnt)
            {
                roleIndex++;
                amntCount = 0;
            }
        }
    }



    public List<(Role role, int howMany)> determineRoleAmounts(Template temp)
    {
        int allCount = 0; int indexToRemember = -1;
        List<(Role role, int howMany)> amounts = new List<(Role role, int howMany)>();

        for(int i = 0; i < temp.rolePool.Count; i++)
        {
            Role r = temp.rolePool[i];
            (ConstantOrProbability min, ConstantOrProbability max) tuple = temp.roleAmounts[r];
            if(tuple.min.rest)
            {
                if(indexToRemember != -1) Debug.LogError("Can't set two roles to have 'rest of' probability.");
                indexToRemember = i;
            } else
            {
                int amount;

                //Constant case
                if (tuple.min.value >= 0)
                {
                    amount = Random.Range(tuple.min.value, tuple.max.value);
                }
                
                //Proportional case
                else
                {
                    amount = Mathf.FloorToInt(Random.Range(tuple.min.probability * size, tuple.max.probability * size));
                }
                allCount += amount;
                amounts.Add((r, amount));
            }
        }

        //if we didn't assign a role to everybody, fill in the rest
        if (allCount < size)
        {
            //Assign everyone else the "default" role
            if (indexToRemember != -1)
            {
                amounts.Add((temp.rolePool[indexToRemember], size - allCount));
            }

            //Just give everybody a random role
            else
            {
                Debug.LogError("Undergenerated roles, no default assigned");
            }
        }
        else if (allCount > size) Debug.LogError("Overgenerated roles");

        return amounts;
    }

    public void DebugLogRoster()
    {
        for (int i = 0; i < roster.Count; i++)
        {
            Debug.Log(roster[i]);
        }
    }
}
