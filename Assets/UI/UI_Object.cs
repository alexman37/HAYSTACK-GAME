using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Any hoverable / clickable UI object
public class UI_Object : MonoBehaviour
{
    public PinpointBoundBox bbox;

    public bool clickable;
    public string hoverText;

    public bool isActive;
    public bool isCurrentlyHovering;

    public bool isHovering()
    {
        return bbox.contains(Input.mousePosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        RectTransform r = GetComponent<Image>().rectTransform;
        Vector2 bottomLeft = offsetFromCenter(r, true);
        Vector2 topRight = offsetFromCenter(r, false);
        bbox = new PinpointBoundBox(bottomLeft, topRight);
        Debug.Log("Bbox " + hoverText + ", BL: " + bottomLeft + ", TR: " + topRight);

        UI_Manager.reportNewObject(this);
    }

    Vector2 offsetFromCenter(RectTransform center, bool bl)
    {
        if (bl)
        {
            return new Vector2(center.position.x - (center.rect.width / 2), center.position.y - (center.rect.height / 2));
        } else
        {
            return new Vector2(center.position.x + (center.rect.width / 2), center.position.y + (center.rect.height / 2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isHovering())
        {
            isCurrentlyHovering = true;
            UI_Manager.enableDescriptorBox(hoverText);
        } else
        {
            if(isCurrentlyHovering)
            {
                isCurrentlyHovering = false;
                UI_Manager.disableDescriptorBox();
            }
        }
    }
}