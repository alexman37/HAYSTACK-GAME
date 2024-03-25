using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



//Handles things all UI need to keep in touch with each other for. Such as:
// The descriptor box
public class UI_Manager : MonoBehaviour
{
    public static GameObject descriptorBox;
    private static Image descriptorBoxImg;

    public static List<int> activeUIObjects;
    public static List<UI_Object> allUIObjects;

    /*public UI_Object isHoveringUIObject(Vector2Int position)
    {
        //Send out a "ping" to all active UI objects, return true if they are
    }*/

    // Start is called before the first frame update
    void Start()
    {
        activeUIObjects = new List<int>();
        descriptorBox = GameObject.FindGameObjectsWithTag("descBox")[0];
        descriptorBoxImg = descriptorBox.GetComponent<Image>();
        Debug.Log(descriptorBox);
    }

    public static void reportNewObject(UI_Object obj)
    {
        if (allUIObjects == null) allUIObjects = new List<UI_Object>();
        allUIObjects.Add(obj);
    }

    public static void enableDescriptorBox(string text)
    {
        if (!descriptorBox.activeInHierarchy) descriptorBox.SetActive(true);
        Vector3 pos = Input.mousePosition;
        pos.x = pos.x + descriptorBoxImg.rectTransform.rect.width / 2 + 10;
        pos.y = pos.y - descriptorBoxImg.rectTransform.rect.height / 2;
        descriptorBoxImg.rectTransform.position = pos;
        descriptorBoxImg.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public static void disableDescriptorBox()
    {
        descriptorBox.SetActive(false);
    }

   
    public static void UIobjectExistsHere(Vector3 pos)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}