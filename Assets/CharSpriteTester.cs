using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharSpriteTester : MonoBehaviour
{
    public Image charachterImgTemplate;
    public Shader colorSwapper;
    private GameObject currImg;

    // Start is called before the first frame update
    void Start()
    {
        Character cp = new Character(0);
        cp.randomizeDemographics();
        Debug.Log(cp.hairColor);
        getNewRandomSprite(cp.skinTone, cp.hairColor);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            getNewRandomSprite(CPD_SkinTone.getRandom(), CPD_HairColor.getRandom());
        }

        //name testing
        if (Input.GetKeyDown(KeyCode.E))
        {
            for(int i = 0; i < 5; i++) {
                (string f, string l) name = CharRandomValue.randomName(true);
                Debug.Log(name.f + " " + name.l);

                name = CharRandomValue.randomName(false);
                Debug.Log(name.f + " " + name.l);
            }
        }
    }

    public void getNewRandomSprite(SkinTone st, HairColor hc)
    {
        Destroy(currImg);

        currImg = Instantiate(charachterImgTemplate.gameObject, this.transform, true);
        currImg.transform.position = new Vector3(100, 100);

        int randHeightOffset = Random.Range(6, 24);

        //create: head (w/ skin), hair (w/ color), face
        charachterImgTemplate.material.SetColor("_NewSkinColor", st.color);
        charachterImgTemplate.material.SetColor("_NewHairColor", hc.color);
        addNewLayerToPortrait("Body");
        addNewLayerToPortrait("Head", randHeightOffset);
        addNewLayerToPortrait("Face", randHeightOffset);
        addNewLayerToPortrait("Hair", randHeightOffset);
    }

    private float rfc()
    {
        return Random.Range(0, 1);
    }

    private void addNewLayerToPortrait(string path)
    {
        Image img = genImage(path, 0);

        Color c = img.color;
        c.a = 1.0f;
        img.color = c;
    }

    private void addNewLayerToPortrait(string path, string shaderVar, Color col)
    {
        Image img = genImage(path, 0);

        img.material.SetColor(shaderVar, col);
        Color c = img.color;
        c.a = 1.0f;
        img.color = c;
    }

    private void addNewLayerToPortrait(string path, int verticalOffset)
    {
        Image img = genImage(path, verticalOffset);

        Color c = img.color;
        c.a = 1.0f;
        img.color = c;
    }

    private Image genImage(string path, int offset)
    {
        List<string> allFiles = getAllFilesInDir("Assets/CharSprites/Resources/CharSprites/" + path);

        Sprite randSpr = Resources.Load<Sprite>("CharSprites/" + path + "/" + allFiles[Mathf.FloorToInt(Random.Range(0, allFiles.Count))].Split('.')[0]);
        //Sprite headSpr = Resources.Load<Sprite>("CharSprites/Head/" + "normal");
        Image Img = Instantiate(charachterImgTemplate, currImg.transform, true);
        Img.sprite = randSpr;
        Img.rectTransform.position = new Vector3(100, 100 + offset);

        return Img;
    }

    private List<string> getAllFilesInDir(string path)
    {
        DirectoryInfo thePath = new DirectoryInfo(path);
        FileInfo[] fileInfo = thePath.GetFiles("*.png", SearchOption.AllDirectories);
        List<string> paths = new List<string>();

        foreach (FileInfo file in fileInfo)
        {
            paths.Add(file.Name);
        }

        return paths;
    }
}
