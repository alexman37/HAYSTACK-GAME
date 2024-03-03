using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Roster : MonoBehaviour
{
    private Roster roster;

    //UI components
    public Image rosterWindow;
    public Image characterCardTemplate;

    private GameObject container;

    public static event Action rosterUIReady;

    void Start()
    {
        createContainer();
        rosterWindow.gameObject.SetActive(false);
        Scenario.rosterCreationDone += setRoster;
        rosterUIReady.Invoke();
    }

    void createContainer()
    {
        container = new GameObject();
        container.transform.SetParent(transform.parent.parent);
        container.transform.position = new Vector3(100, 100, 0);
    }

    void setRoster(Roster rost)
    {
        Debug.Log("set roster");
        roster = rost;
    }

    public void toggleRosterWindow()
    {
        bool newVal = !rosterWindow.gameObject.activeInHierarchy;
        if (newVal == true) generateAllCharCards();
        else
        {
            Destroy(container);
            createContainer();
        }

        rosterWindow.gameObject.SetActive(newVal);
    }

    public void generateAllCharCards()
    {
        int entriesPerRow = 10;
        float startingX = characterCardTemplate.rectTransform.position.x;
        float startingY = characterCardTemplate.rectTransform.position.y;
        float cardWidth = characterCardTemplate.rectTransform.rect.width;
        float cardHeight = characterCardTemplate.rectTransform.rect.height;
        float cardOffsetW = cardWidth / 10f;
        float cardOffsetH = cardHeight / 10f;


        for (int i = 0; i < roster.roster.Count; i++)
        {
            Character c = roster.roster[i];

            //instantiate card in correct position
            Image newCard = GameObject.Instantiate(characterCardTemplate);
            newCard.transform.SetParent(container.transform);
            newCard.rectTransform.position = new Vector3(
                startingX + Mathf.Floor(i % entriesPerRow) * (cardWidth + cardOffsetW), 
                startingY - Mathf.Floor(i / entriesPerRow) * (cardHeight + cardOffsetH), 0);
            newCard.gameObject.SetActive(true);
            newCard.sprite = roster.rosterSprites[i];

            //set portrait and name
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = c.getDisplayName(true);
        }
    }
}
