using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop : MonoBehaviour {
    public List<UICard> allCards; // These are just empty GameObjects used for Instantiation
    public List<BaseEntity> warband = new List<BaseEntity>(); // To be updated upon click of "End Turn" Button?
    public TextMeshProUGUI money; //Display the amount of money available
    
    private EntitiesDatabaseSO cachedDb;
    private int entitiyCost = 3;
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
                if(!allCards[i].transform.parent.transform.GetChild(0).gameObject.GetComponent<BaseEntity>().isFrozen) {
                    Destroy(allCards[i].transform.parent.transform.GetChild(0).gameObject);
                    allCards[i].Setup(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
                }
            } else {
                allCards[i].Setup(cachedDb.allEntities[Random.Range(0, cachedDb.allEntities.Count)], this);
            }
        }
    }
    
    public void OnRerollClick() {
        //Check if can afford, then decrease money and generate new cards
        if(PlayerData.Instance.CanAfford(rerollCost)) {
            PlayerData.Instance.SpendMoney(rerollCost);
            GenerateCard();
        }
    }

    public bool OnDragToWarband() {
        bool ans = AllowDragToWarband();
        if(ans) {
            PlayerData.Instance.SpendMoney(entitiyCost);
        }
        return ans;
    }

    bool AllowDragToWarband() {
        return PlayerData.Instance.CanAfford(entitiyCost);
    }

    void Refresh() {
        money.text = PlayerData.Instance.Money.ToString();
    }
}
