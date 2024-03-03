using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPD_BodyType : MonoBehaviour
{
    private static float probCounter = 0.0f;
    private static float probX = 0.0f;

    public static List<BodyType> variants = initialize();

    private static List<BodyType> initialize()
    {
        TextAsset txt = Resources.Load<TextAsset>("bodyTypesTxt");
        string[] lines = txt.text.Split('\n');
        List<BodyType> vars = new List<BodyType>();
        List<BodyType> temporaryStorage = new List<BodyType>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Split('#')[0].Trim(); //ignore comments
            if (line.Length > 0)
            {
                string[] fields = line.Split(';');

                //probability
                float p;
                if (fields[2] == "X")
                {
                    probX += 1.0f;
                    p = -1;
                }
                else
                {
                    p = float.Parse(fields[2]);
                    probCounter += p;
                }

                BodyType st = new BodyType(i, "CharSprites/Body/" + fields[0], fields[1], p, fields[3]);
                temporaryStorage.Add(st);
            }
        }

        if (probCounter > 1)
        {
            Debug.LogError("Failed to use BodyTypes, probabilities do not equal 1");
            return null;
        }
        else if (probX > 0)
        {
            probX = (1 - probCounter) / probX;
        }

        foreach (BodyType st in temporaryStorage)
        {
            if (st.probability == -1)
            {
                st.probability = probX;
            }

            vars.Add(st);
        }

        return vars;
    }

    public static BodyType getRandom()
    {
        return variants[Random.Range(0, variants.Count)];
    }

}

//define the characteristics of this property here
public class BodyType
{
    int id;
    public string filename;
    public string name;
    public float probability;
    public string generalDesc;

    public BodyType(int id, string filename, string name, float probability, string general)
    {
        this.id = id;
        this.filename = filename;
        this.name = name;
        this.probability = probability;
        this.generalDesc = general;
    }

    public Sprite getSprite()
    {
        return Resources.Load<Sprite>(filename);
    }

    public override string ToString()
    {
        return this.name;
    }
}
