using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CPD_HairColor
{
    private static float probCounter = 0.0f;
    private static float probX = 0.0f;

    public static List<HairColor> variants = initialize();

    private static List<HairColor> initialize()
    {
        TextAsset txt = Resources.Load<TextAsset>("hairTonesTxt");
        string[] lines = txt.text.Split('\n');
        List<HairColor> vars = new List<HairColor>();
        List<HairColor> temporaryStorage = new List<HairColor>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Split('#')[0].Trim(); //ignore comments
            if (line.Length > 0)
            {
                string[] fields = line.Split(';');

                //color
                float[] colors = new float[3];
                string[] strColors = fields[2].Split(',');
                for (int s = 0; s < strColors.Length; s++) {
                    string curr = strColors[s];
                    if(strColors[s].Contains("-"))
                    {
                        string[] _ = curr.Split('-');
                        colors[s] = Random.Range(int.Parse(_[0]), int.Parse(_[1])) / 255.0f;
                    }
                    else if(strColors[s].Contains("X"))
                    {
                        if (strColors[s] == "X0") colors[s] = colors[0];
                        if (strColors[s] == "X1") colors[s] = colors[1];
                    }
                    else colors[s] = int.Parse(strColors[s]) / 255.0f;
                }
                Color col = new Color(colors[0], colors[1], colors[2]);

                //probability
                float p;
                if (fields[1] == "X")
                {
                    probX += 1.0f;
                    p = -1;
                }
                else
                {
                    p = float.Parse(fields[1]);
                    probCounter += p;
                }

                HairColor st = new HairColor(i, col, fields[0], p, fields[3]);
                temporaryStorage.Add(st);
            }
        }

        if (probCounter > 1)
        {
            Debug.LogError("Failed to use HairColors, probabilities do not equal 1");
            return null;
        }
        else if (probX > 0)
        {
            probX = (1 - probCounter) / probX;
        }

        foreach (HairColor st in temporaryStorage)
        {
            if (st.probability == -1)
            {
                st.probability = probX;
            }

            vars.Add(st);
        }

        return vars;
    }

    public static HairColor getRandom()
    {
        return variants[Random.Range(0, variants.Count)];
    }

}

//define the characteristics of this property here
public class HairColor
{
    int id;
    public Color32 color;
    public string name;
    public float probability;
    public string general;

    public HairColor(int id, Color32 color, string name, float probability, string general)
    {
        this.id = id;
        this.color = color;
        this.name = name;
        this.probability = probability;
        this.general = general;
    }

    public override string ToString()
    {
        return this.name;
    }
}