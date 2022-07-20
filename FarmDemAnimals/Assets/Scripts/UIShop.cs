using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop : MonoBehaviour {
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
    /// <remarks>
    /// Incorporates the Animal Tiering System
    /// </remarks>
    /// <returns>
    /// Struct of EntitiesDatabaseSO.EntityData that contains the prefab reference of the animal
    /// </returns>
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
    /// </summary>
    /// <remarks>
    /// Called upon clicking REROLL button
    /// </remarks>
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
                        allCards[i].Setup(ChooseAnimalFromDatabase(), this, false); 
                    }
                } else {
                    allCards[i].Setup(ChooseAnimalFromDatabase(), this, false);
                }
            }
        }
    }

    /// <summary>
    /// Generates previously frozen animals through ShopDataSO.frozenShopEntities
    /// Generates the remaining animals returned by ChooseAnimalFromDatabase() and Instantiates into the game through UICard.Setup()
    /// </summary>
    /// <remarks>
    /// Called ONLY at the START of each PREPARATION PHASE
    /// </remarks>
    private void GenerateCardsAtStartOfTurn() {
        //Debug.Log("GenerateCardsAtStartOfTurn() called");
        // Fill up slots with previously frozen animals first
        if (PlayerData.Instance.TurnNumber > 1) {
            foreach (ShopDataSO.EntityData animalInfo in shopData.frozenShopEntities) {
                // Grab the base Prefab based on animalID from entitiesDatabaseSO
                EntitiesDatabaseSO.EntityData frozenAnimal = entitiesDatabase.allEntities[animalInfo.animalID - 1];
                // Generate the animal in correct position
                allCards[animalInfo.position].Setup(frozenAnimal, this, true);
                Debug.Log("Generating previously frozen animal: " + frozenAnimal.prefab.name);
            }
        }
        
        // Fill up the rest of the available slots
        for (int i = 0; i < allCards.Count; i++) {
            // If slot is active
            if(allCards[i].CanGenerate()) { 
                // If slot is empty
                if (allCards[i].transform.parent.transform.childCount == 1) {
                    allCards[i].Setup(ChooseAnimalFromDatabase(), this, false);
                }
            }
        }
    }
    
    /// <summary>
    /// Checks if player has sufficient money to reroll
    /// If allowed, rerolls Shop Animals by calling RerollCards()
    /// Reduces the Gold Amount by 1
    /// </summary>
    public void OnRerollClick() {
        //Check if can afford, then decrease money and generate new cards
        if(PlayerData.Instance.CanAfford(rerollCost)) {
            //Debug.Log("Rerolled");
            PlayerData.Instance.SpendMoney(rerollCost);
            RerollCards();
        } else {
            SoundManager.Instance.Play("Error");
        }
    }

    /// <summary>
    /// Changes current state to "TURNEND"
    /// Proceeds to change scenes to Battle Scene
    /// </summary>
    public void OnEndTurnClick() {
        PreparationManager.Instance.ChangeState(PreparationManager.CurrentState.TURNEND);
        SoundManager.Instance.Play("Click");
        // Go to next scene ==> Should implement some sort of wait first
        SceneController.Instance.LoadScene("Scenes/Battle Scene");
    }

    /// <summary>
    /// Checks if player has sufficient money to buy an animal through AllowDragToWarband()
    /// If allowed, reduces the Gold Amount by 3
    /// </summary>
    /// <returns>
    /// Boolean representing whether the player is allowed to buy an animal
    /// </returns>
    public bool OnDragToWarband() {
        //Debug.Log("Bought");
        bool ans = AllowDragToWarband();
        if(ans) {
            PlayerData.Instance.SpendMoney(entitiyCost);
        } else {
            SoundManager.Instance.Play("Error");
        }
        return ans;
    }

    /// <summary>
    /// Increases Gold Amount by the totalEntityCount of the sold animal
    /// </summary>
    public void SellSuccess(int totalEntityCount) {
        //Debug.Log("Sold " + totalEntityCount);
        PlayerData.Instance.AddMoney(sellCost * totalEntityCount);
    }

    /// <summary>
    /// Helper function for OnDragToWarband()
    /// Checks if player has sufficient money to reroll
    /// </summary>
    /// <returns>
    /// Boolean representing whether the player is allowed to buy an animal
    /// </returns>
    private bool AllowDragToWarband() {
        return PlayerData.Instance.CanAfford(entitiyCost);
    }

    /// <summary>
    /// Function that listens to the OnUpdateMoney event
    /// Called whenever OnUpdateMoney is invoked
    /// Updates the Gold Amount to the correct amount
    /// </summary>
    public void Refresh() {
        money.text = PlayerData.Instance.Money.ToString();
    }

    // -------------------------------- DEBUG BUTTONS --------------------------------------
    /// For DEBUG only
    /*public void OnMainMenuClick() {
        SceneController.Instance.LoadScene("Scenes/Main Menu");
    }*/
}
