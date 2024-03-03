using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//IF THIS TURNS OUT TO BE BROKEN, JUST MAKE A "FETCH TEMPLATE" method for each template
public static class AllTemplates
{
    public static Template[] templateList = defineTemplates();

    //DEFINE TEMPLATES HERE
    //The reason we pass in probabilities, not calculate a random value, is so that they can be random every time
    public static Template[] defineTemplates()
    {
        templateList = new Template[1];

        //1. TRAIN
        (Role role, ConstantOrProbability min, ConstantOrProbability max)[] roleProbsTrain =
        {
            (new Role("Conductor", 5), cons(1), cons(1)),
            (new Role("Staff", 3), prob(0.08f), prob(0.16f)),
            (new Role("Passenger", 5), cons(-1), cons(-1)),
        };
        templateList[0] = new Template("Train",
            roleProbsTrain);

        return templateList;
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