using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrait
{
    //Define all traits here.
    //Sort them by how "important" they are!
    private static CharacterTrait[] allTraits =
    {
        new CharacterTrait("Psychopath", 0.02f),
        new CharacterTrait("Schizo", 0.05f),
        new CharacterTrait("Disavowed", 0.07f),
        new CharacterTrait("Famous", 0.05f),
        new CharacterTrait("Doppelganger", 0.02f),
        new CharacterTrait("Troll", 0.05f),
        new CharacterTrait("Cold", 0.1f),
        new CharacterTrait("Confrontational", 0.05f),
        new MutuallyExclusiveTrait("Keen", "Forgetful", 0.2f),
        new MutuallyExclusiveTrait("Extrovert", "Introvert", 0.3f),
        new MutuallyExclusiveTrait("Popular", "Lone Wolf", 0.15f),
        new MutuallyExclusiveTrait("Strong", "Feeble", 0.2f),
        new MutuallyExclusiveTrait("Queasy", "Bloodlust", 0.2f, 0.15f),
        new MutuallyExclusiveTrait("Moronic", "Genius", 0.1f),
        new MutuallyExclusiveTrait("Religious", "Satanic", 0.05f),
        new MutuallyExclusiveTrait("Honest", "Dishonest", 0.2f),
        new MutuallyExclusiveTrait("Adventurous", "Reclusive", 0.12f),
    };
    private static int size = allTraits.Length;


    //Instance level
    public string name; //Name of trait
    public float prob; //Probability it is assigned to a character

    public CharacterTrait(string name, float prob)
    {
        this.name = name;
        this.prob = prob;
    }

    //Go down the list of traits. Run probability calculations for them.
    //There's a chance you end up with exactly enough traits, or even none at all.
    public static HashSet<string> getRandomTraits(int maxAllowed)
    {
        HashSet<string> traits = new HashSet<string>();

        for(int i = 0; i < size; i++)
        {
            CharacterTrait ct = allTraits[i];
            if (Random.value < ct.prob)
            {
                if(ct is MutuallyExclusiveTrait) traits.Add((allTraits[i] as MutuallyExclusiveTrait).getVariant());
                else traits.Add(allTraits[i].name);
            }
            if (traits.Count >= maxAllowed) break;
        }

        return traits;
    }
}

public class MutuallyExclusiveTrait : CharacterTrait
{
    public string otherName;
    public float probPositive;

    //With specific probPositive
    public MutuallyExclusiveTrait(string name1, string name2, float probPositive, float prob) : base(name1, prob)
    {
        this.otherName = name2;
        this.probPositive = probPositive;
    }

    //Default probPositive to a coin toss
    public MutuallyExclusiveTrait(string name1, string name2, float prob) : base(name1, prob)
    {
        this.otherName = name2;
        this.probPositive = 0.5f;
    }

    public string getVariant()
    {
        if (Random.value < probPositive) return name;
        else return otherName;
    }
}