using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop : MonoBehaviour {
    //public List<UICard> allCards; // Contains empty GameObjects used for Instantiation (Assigned in Editor)
    //public List<BaseEntity> warband = new List<BaseEntity>(); // To be updated upon click of "End Turn" Button?
    public TextMeshProUGUI money; //Display the amount of money available
    
    private EntitiesDatabaseSO entitiesDatabase;
    private List<UICard> allCards;
    private int entitiyCost = 3;
    private int rerollCost = 1;
    private int sellCost = 1;

    private void Start() {
        Debug.Log("Enter UIShop Start()");
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        PreparationManager.Instance.ActivateShopSlots();
        allCards = PreparationManager.Instance.allCards;
        GenerateCard();
        PlayerData.Instance.OnUpdateMoney += Refresh; // Everytime a function calls OnUpdateMoney?.Invoke(), Refresh() is called
        Refresh();
    }

    private EntitiesDatabaseSO.EntityData ChooseAnimalFromDatabase() {
        Debug.Log("Enter UIShop ChooseAnimalFromDatabase()");
        EntitiesDatabaseSO.EntityData randomAnimal = entitiesDatabase.allEntities[Random.Range(0, entitiesDatabase.allEntities.Count)];
        while (randomAnimal.prefab.GetAnimalTier() > PlayerData.Instance.TurnNumber) {
            randomAnimal = entitiesDatabase.allEntities[Random.Range(0, entitiesDatabase.allEntities.Count)];
        }
        return randomAnimal;
    }

    public void GenerateCard() {
        Debug.Log("Enter UIShop GenerateCard()");
        for (int i = 0; i < allCards.Count; i++) {
            if(allCards[i].CanGenerate()) { // If slot is active
                if(allCards[i].transform.parent.transform.childCount > 1) { // If animal in slot
                    if(!allCards[i].transform.parent.transform.GetChild(0).gameObject.GetComponent<BaseEntity>().isFrozen) { // If not frozen
                        Destroy(allCards[i].transform.parent.transform.GetChild(0).gameObject); // Destroy animal
                        allCards[i].Setup(ChooseAnimalFromDatabase(), this); //Generate new animal
                    }
                } else {
                    allCards[i].Setup(ChooseAnimalFromDatabase(), this);
                }
            }
        }
    }
    
    public void OnRerollClick() {
        //Check if can afford, then decrease money and generate new cards
        if(PlayerData.Instance.CanAfford(rerollCost)) {
            //Debug.Log("Rerolled");
            PlayerData.Instance.SpendMoney(rerollCost);
            GenerateCard();
        }
    }

    public void OnEndTurnClick() {
        PreparationManager.Instance.ChangeState(PreparationManager.CurrentState.TURNEND);
        // Go to next scene ==> Should implement some sort of wait first
        SceneController.Instance.LoadScene("Scenes/Battle Scene");
    }

    public void OnMainMenuClick() {
        SceneController.Instance.LoadScene("Scenes/Main Menu");
    }

    public bool OnDragToWarband() {
        //Debug.Log("Bought");
        bool ans = AllowDragToWarband();
        if(ans) {
            PlayerData.Instance.SpendMoney(entitiyCost);
        }
        return ans;
    }

    public void SellSuccess(int totalEntityCount) {
        Debug.Log("Sold " + totalEntityCount);
        PlayerData.Instance.AddMoney(sellCost * totalEntityCount);
    }

    bool AllowDragToWarband() {
        return PlayerData.Instance.CanAfford(entitiyCost);
    }

    void Refresh() {
        money.text = PlayerData.Instance.Money.ToString();
    }
}
