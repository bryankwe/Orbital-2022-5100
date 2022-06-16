using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour {
    public List<UICard> allCards;
    //public Text money; //Display the amount of money available
    
    private EntitiesDatabaseSO cachedDb;
    //private int entitiyCost = 3;
    //private int rerollCost = 1;

    private void Start() {
        cachedDb = GameManager.Instance.entitiesDatabase;
        GenerateCard();
    }

    public void GenerateCard() {
        for (int i = 0; i < allCards.Count; i++) {
            if(allCards[i].transform.parent.transform.childCount > 1) {
                Destroy(allCards[i].transform.parent.transform.GetChild(0).gameObject);
            }
            allCards[i].Setup(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
        }
    }

    public void OnRerollClick() {
        //Check if can afford, then decrease money and generate new cards
        GenerateCard();
    }

    /*public void OnCardClick(EntitiesDatabaseSO.EntityData cardData) {
        Debug.Log("Card clicked!");
    }*/
}
