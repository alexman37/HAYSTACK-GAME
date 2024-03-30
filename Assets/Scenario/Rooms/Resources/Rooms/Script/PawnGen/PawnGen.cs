using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PawnGen
{
    //Generate sprite associated with this character. We'd like to store it somewhere
    //SO we only have to do this once.
    private static Dictionary<string, List<string>> spriteLists = generateAllPawnSpriteLists();


    public static Sprite genSpriteFromLayers(Character ch)
    {
        Color[] colsToReplace = { Color.red, Color.blue, Color.green };
        Color[] colsOfBody = { ch.skinTone.color, Color.cyan, Color.gray };
        Sprite b = getRandomPawnResource("BodyTypes");
        Sprite h = getRandomPawnResource("HairTypes");
        SpriteGenLayer body = new SpriteGenLayer(b, CharSpriteGen.fromList(colsToReplace), CharSpriteGen.fromList(colsOfBody));
        SpriteGenLayer hair = new SpriteGenLayer(h, CharSpriteGen.oneList(Color.red), CharSpriteGen.oneList(ch.hairColor.color), 
            constantOffset(b.texture, h.texture, Color.white));

        SpriteGenLayer[] newLayers = { body, hair };

        Texture2D newTex = new Texture2D(120, 120);
        newTex = cleanCanvas(newTex);
        newTex = CharSpriteGen.addLayers(newTex, newLayers);
        return Sprite.Create(newTex, new Rect(0, 0, 120, 120), new Vector2(0f, 0f));
    }

    public static Sprite generatePawnFromChar(Character ch)
    {
        return genSpriteFromLayers(ch);
    }


    private static Sprite getRandomPawnResource(string pathEnd)
    {
        string p = "Rooms/Pawns/" + pathEnd + "/" + spriteLists[pathEnd][Random.Range(0, spriteLists[pathEnd].Count)].Split('.')[0];
        return Resources.Load<Sprite>(p);
    }




    private static Texture2D cleanCanvas(Texture2D starter)
    {
        for (int x = 0; x < starter.width; x++)
        {
            for (int y = 0; y < starter.height; y++)
            {
                starter.SetPixel(x, y, Color.clear);
                
            }
        }
        starter.Apply();
        return starter;
    }

    //Initial list gen (do for ALL)
    private static Dictionary<string, List<string>> generateAllPawnSpriteLists()
    {
        List<string> bodySprites = generatePawnSpriteList("BodyTypes");
        List<string> hairSprites = generatePawnSpriteList("HairTypes");

        Dictionary<string, List<string>> spriteLists = new Dictionary<string, List<string>>();
        spriteLists.Add("BodyTypes", bodySprites);
        spriteLists.Add("HairTypes", hairSprites);
        return spriteLists;
    }

    //DO for one
    private static List<string> generatePawnSpriteList(string pathEnd)
    {
        List<string> temp = new List<string>();

        foreach (FileInfo file in new DirectoryInfo("Assets/Scenario/Rooms/Resources/Rooms/Pawns/" + pathEnd).GetFiles("*.png"))
        {
            temp.Add(file.Name);
        }

        return temp;
    }

    //TODO: Brutally inefficient, but we'll deal with it for now
    private static (int xOffset, int yOffset) constantOffset(Texture2D show, Texture2D below, Color matchColor)
    {
        (int x, int y) showPos = (-99,-99);
        (int x, int y) belowPos = (-99,-99);
        for (int x = 0; x < show.width; x++)
        {
            for (int y = 0; y < show.height; y++)
            {
                if (show.GetPixel(x, y) == matchColor) showPos = (x, y);
                if (below.GetPixel(x, y) == matchColor) belowPos = (x, y);
                if (showPos.x != -99 && belowPos.x != -99) return (showPos.x - belowPos.x, showPos.y - belowPos.y);
            }
        }
        return (0, 0);

    }
}