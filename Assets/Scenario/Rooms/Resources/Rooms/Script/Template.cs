using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template
{

    public string name;
    public List<Role> rolePool;

    //Tuple represents min and max amount fo people who can have this role
    //Use (X,X) to always have a constant amount of this role
    //Use (-1,-1) to make everybody this role once the others have been chosen.
    public Dictionary<Role, (ConstantOrProbability min, ConstantOrProbability max)> roleAmounts;

    public Template(string name, (Role role, ConstantOrProbability min, ConstantOrProbability max)[] roles)
    {
        this.name = name;

        rolePool = new List<Role>();
        roleAmounts = new Dictionary<Role, (ConstantOrProbability min, ConstantOrProbability max)>();

        int saveTheIndex = -1;
        for(int i = 0; i < roles.Length; i++)
        {
            (Role role, ConstantOrProbability min, ConstantOrProbability max) r = roles[i];
            rolePool.Add(r.role);
            if(r.min.rest)
            {
                if (saveTheIndex != -1) Debug.LogError("Can't set two roles to have 'rest of' probability.");
                saveTheIndex = i;
                roleAmounts.Add(r.role, (r.min, r.max));
            }
            else roleAmounts.Add(r.role, (r.min, r.max));
        }
    }



    //Use when you know exactly how much of this role you want
    private static ConstantOrProbability cons(int v)
    {
        return new ConstantOrProbability(v);
    }

    //Use when you want the amount to be proportional
    private static ConstantOrProbability prob(float p)
    {
        return new ConstantOrProbability(p);
    }
}



public class ConstantOrProbability
{
    public int value = 0;
    public float probability = -1;
    public bool rest = false;

    public ConstantOrProbability(int v)
    {
        value = v;
        if (v == -1) rest = true;
    }

    public ConstantOrProbability(float p)
    {
        probability = p;
    }
}