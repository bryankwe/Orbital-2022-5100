using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop : MonoBehaviour {
    public List<UICard> allCards;
    public TextMeshProUGUI money; //Display the amount of money available
    
    private EntitiesDatabaseSO cachedDb;
    //private int entitiyCost = 3;
    private int rerollCost = 1;

    private void Start() {
        cachedDb = GameManager.Instance.entitiesDatabase;
        GenerateCard();
        PlayerData.Instance.OnUpdate += Refresh;
        Refresh();
    }

    public void GenerateCard() {
        for (int i = 0; i < allCards.Count; i++) {
            if(allCards[i].transform.parent.transform.childCount > 1) {
                Destroy(allCards[i].transform.parent.transform.GetChild(0).gameObject);
            }
            allCards[i].Setup(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
        }
    }

    /*public bool AllowDragToWarband() {
        return PlayerData.Instance.CanAfford(entitiyCost);
    }*/
    
    public void OnRerollClick() {
        //Check if can afford, then decrease money and generate new cards
        if(PlayerData.Instance.CanAfford(rerollCost)) {
            PlayerData.Instance.SpendMoney(rerollCost);
            GenerateCard();
        }
    }

    void Refresh() {
        money.text = PlayerData.Instance.Money.ToString();
    }

    /*public void OnCardClick(EntitiesDatabaseSO.EntityData cardData) {
        Debug.Log("Card clicked!");
    }*/
}
