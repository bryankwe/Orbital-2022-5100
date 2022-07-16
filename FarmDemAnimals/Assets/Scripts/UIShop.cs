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
    private ShopDataSO shopData;
    private List<UICard> allCards;
    private int entitiyCost = 3;
    private int rerollCost = 1;
    private int sellCost = 1;

    private void Start() {
        //Debug.Log("Enter UIShop Start()");
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        shopData = GameManager.Instance.shopData;
        PreparationManager.Instance.ActivateShopSlots();
        allCards = PreparationManager.Instance.allCards;
        GenerateCardsAtStartOfTurn();
        PlayerData.Instance.OnUpdateMoney += Refresh; // Everytime a function calls OnUpdateMoney?.Invoke(), Refresh() is called
        Refresh();
    }

    /// <summary>
    /// Randomly Selects an Animal of the Correct (Allowable) Tier from EntitiesDatabaseSO
    /// </summary>
    private EntitiesDatabaseSO.EntityData ChooseAnimalFromDatabase() {
        //Debug.Log("Enter UIShop ChooseAnimalFromDatabase()");
        EntitiesDatabaseSO.EntityData randomAnimal = entitiesDatabase.allEntities[Random.Range(0, entitiesDatabase.allEntities.Count)];
        while (randomAnimal.prefab.GetAnimalTier() > PlayerData.Instance.TurnNumber) {
            randomAnimal = entitiesDatabase.allEntities[Random.Range(0, entitiesDatabase.allEntities.Count)];
        }
        return randomAnimal;
    }

    /// <summary>
    /// Generates the card returned by ChooseAnimalFromDatabase() and Instantiates into the game through UICard.Setup()
    /// Called when rerolling the cards
    /// </summary>
    private void RerollCards() {
        //Debug.Log("RerollCards() called");
        for (int i = 0; i < allCards.Count; i++) {
            // If slot is active
            if(allCards[i].CanGenerate()) { 
                // If animal in slot
                if(allCards[i].transform.parent.transform.childCount > 1) {
                    // If animal is not frozen
                    if(!allCards[i].transform.parent.transform.GetChild(0).gameObject.GetComponent<BaseEntity>().isFrozen) {
                        // Destroy existing animal in slot (that is not frozen)
                        Destroy(allCards[i].transform.parent.transform.GetChild(0).gameObject);
                        // Generate new animal in slot
                        allCards[i].Setup(ChooseAnimalFromDatabase(), this); 
                    }
                } else {
                    allCards[i].Setup(ChooseAnimalFromDatabase(), this);
                }
            }
        }
    }

    /// <summary>
    /// Generates previously frozen animals through ShopDataSO.frozenShopEntities
    /// Generates the remaining animals returned by ChooseAnimalFromDatabase() and Instantiates into the game through UICard.Setup()
    /// Called only at the start of each preparation phase
    /// </summary>
    private void GenerateCardsAtStartOfTurn() {
        //Debug.Log("GenerateCardsAtStartOfTurn() called");
        // Fill up slots with previously frozen animals first
        if (PlayerData.Instance.TurnNumber > 1) {
            foreach (ShopDataSO.EntityData animalInfo in shopData.frozenShopEntities) {
                // Grab the base Prefab based on animalID from entitiesDatabaseSO
                EntitiesDatabaseSO.EntityData frozenAnimal = entitiesDatabase.allEntities[animalInfo.animalID - 1];
                // Generate the animal in correct position
                allCards[animalInfo.position].Setup(frozenAnimal, this);
                Debug.Log("Generating previously frozen animal: " + frozenAnimal.prefab.name);
            }
        }
        
        // Fill up the rest of the available slots
        for (int i = 0; i < allCards.Count; i++) {
            // If slot is active
            if(allCards[i].CanGenerate()) { 
                // If slot is empty
                if (allCards[i].transform.parent.transform.childCount == 1) {
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
            RerollCards();
        }
    }

    public void OnEndTurnClick() {
        PreparationManager.Instance.ChangeState(PreparationManager.CurrentState.TURNEND);
        // Go to next scene ==> Should implement some sort of wait first
        SceneController.Instance.LoadScene("Scenes/Battle Scene");
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
        //Debug.Log("Sold " + totalEntityCount);
        PlayerData.Instance.AddMoney(sellCost * totalEntityCount);
    }

    bool AllowDragToWarband() {
        return PlayerData.Instance.CanAfford(entitiyCost);
    }

    public void Refresh() {
        money.text = PlayerData.Instance.Money.ToString();
    }

    // -------------------------------- DEBUG BUTTONS --------------------------------------
    /// For DEBUG only
    /*public void OnMainMenuClick() {
        SceneController.Instance.LoadScene("Scenes/Main Menu");
    }*/
}
