using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPD_HeadType : MonoBehaviour
{
    private static float probCounter = 0.0f;
    private static float probX = 0.0f;

    public static List<HeadType> variants = initialize();

    private static List<HeadType> initialize()
    {
        TextAsset txt = Resources.Load<TextAsset>("headTypesTxt");
        string[] lines = txt.text.Split('\n');
        List<HeadType> vars = new List<HeadType>();
        List<HeadType> temporaryStorage = new List<HeadType>();

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

                HeadType st = new HeadType(i, "CharSprites/Head/" + fields[0], fields[1], p, fields[3]);
                temporaryStorage.Add(st);
            }
        }

        if (probCounter > 1)
        {
            Debug.LogError("Failed to use HeadTypes, probabilities do not equal 1");
            return null;
        }
        else if (probX > 0)
        {
            probX = (1 - probCounter) / probX;
        }

        foreach (HeadType st in temporaryStorage)
        {
            if (st.probability == -1)
            {
                st.probability = probX;
            }

            vars.Add(st);
        }

        return vars;
    }

    public static HeadType getRandom()
    {
        return variants[Random.Range(0, variants.Count)];
    }

}

//define the characteristics of this property here
public class HeadType
{
    int id;
    public string filename;
    public string name;
    public float probability;
    public string generalDesc;

    public HeadType(int id, string filename, string name, float probability, string general)
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
