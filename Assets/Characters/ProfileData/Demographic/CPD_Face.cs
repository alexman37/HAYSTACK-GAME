using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPD_Face : MonoBehaviour
{
    private static float probCounter = 0.0f;
    private static float probX = 0.0f;

    public static List<FaceType> variants = initialize();

    private static List<FaceType> initialize()
    {
        TextAsset txt = Resources.Load<TextAsset>("faceTypesTxt");
        string[] lines = txt.text.Split('\n');
        List<FaceType> vars = new List<FaceType>();
        List<FaceType> temporaryStorage = new List<FaceType>();

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

                FaceType st = new FaceType(i, "CharSprites/Face/" + fields[0], fields[1], p, fields[3]);
                temporaryStorage.Add(st);
            }
        }

        if (probCounter > 1)
        {
            Debug.LogError("Failed to use FaceTypes, probabilities do not equal 1");
            return null;
        }
        else if (probX > 0)
        {
            probX = (1 - probCounter) / probX;
        }

        foreach (FaceType st in temporaryStorage)
        {
            if (st.probability == -1)
            {
                st.probability = probX;
            }

            vars.Add(st);
        }

        return vars;
    }

    public static FaceType getRandom()
    {
        return variants[Random.Range(0, variants.Count)];
    }

}

//define the characteristics of this property here
public class FaceType
{
    int id;
    public string filename;
    public string name;
    public float probability;
    public string generalDesc;

    public FaceType(int id, string filename, string name, float probability, string general)
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
