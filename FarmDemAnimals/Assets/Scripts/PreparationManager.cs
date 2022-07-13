using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationManager : Manager<PreparationManager> {
    
    private EntitiesDatabaseSO entitiesDatabase;
    private WarbandDataSO warbandData;
    private ShopDataSO shopData;
    private EnemyDatabaseSO enemyDatabase;
    public UIShop shopRef;
    public bool isTesting = false; // Remember to toggle this if debugging / testing
    public List<UICard> allCards; // Contains empty GameObjects used for Instantiation in Shop (Assigned in Editor)
    public List<Slot> warbandSlots; // Contains Slots to retrieve Animals in Warband (Assigned in Editor)
    public List<BaseEntity> warband = new List<BaseEntity>(); // Actual List of Animals in Warband (Updated with every Change)

    public CurrentState currentState;

    public System.Action OnUpdateWarband;
    /*public System.Action OnBuy;
    public System.Action OnSell;
    public System.Action OnTurnStart;
    public System.Action OnTurnEnd;
    public System.Action OnCombine;*/

    
    private void Start() {
        //Debug.Log("Enter PrepManager Start()");
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        OnUpdateWarband += UpdateWarband;
        UpdateWarband();
        warbandData = GameManager.Instance.warbandData;
        shopData = GameManager.Instance.shopData;
        enemyDatabase = GameManager.Instance.enemyDatabase;
        ChangeState(CurrentState.TURNSTART);
    }

    /// <summary>
    /// Update the animals in the warband
    /// </summary>
    private void UpdateWarband() {
        // Debug.Log("Enter PrepManager UpdateWarband()");
        warband = new List<BaseEntity>();
        foreach (Slot slot in warbandSlots) {
            if (slot.transform.childCount > 0) { // If animal in slot
                if (slot.transform.GetChild(0).gameObject.TryGetComponent(out BaseEntity animal)) { // Always true
                    warband.Add(animal);
                }
            } else {
                warband.Add(null);
            }
        }
    }

    /// <summary>
    /// Instantiates Warband Animals after BATTLE Phase
    /// </summary>
    private void RestoreWarbandAnimals() {
        // Not required for the first turn
        if (PlayerData.Instance.TurnNumber == 1) {
            return;
        }

        foreach (WarbandDataSO.EntityData animalInfo in warbandData.warbandEntities) {

            // Grab the base Prefab based on animalID from entitiesDatabaseSO
            BaseEntity actualPrefab = entitiesDatabase.allEntities[animalInfo.animalID - 1].prefab;
            // Instantiate at respective position
            BaseEntity newCard = Instantiate(actualPrefab, 
                                            warbandSlots[animalInfo.position].transform.position, 
                                            Quaternion.identity);
            newCard.transform.SetParent(warbandSlots[animalInfo.position].transform);
            // Edit instantiated animal to suit Preparation Scene
            newCard.gameObject.GetComponent<DragHandler>().typeOfItem = DragHandler.Origin.WARBAND; // Set the Origin to WARBAND
            newCard.shopRef = shopRef; // Set the shopRef to the current shop
            newCard.totalEntityCount = animalInfo.totalEntityCount; // Set the correct totalEntityCount
            newCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Change Scale
            newCard.SetStats(animalInfo.attack, animalInfo.health); // Update Stats Accordingly
        }
        OnUpdateWarband?.Invoke();
    }
    
    /// <summary>
    /// Activate any START OF TURN special abilities
    /// </summary>
    private void StartTurn() {
        // Debug.Log("Enter PrepManager StartTurn()");
        foreach (BaseEntity baseEntity in warband) {
            if (baseEntity != null) {
                Debug.Log("Checked: " + baseEntity.name);
                if (baseEntity.ability == BaseEntity.Ability.TURNSTART) {
                    baseEntity.activateAbility();
                }
            }
        }
        OnUpdateWarband?.Invoke();
        ChangeState(CurrentState.PREPARE);
    }

    /// <summary>
    /// Activate any END OF TURN special abilities
    /// </summary>
    private void EndTurn() {
        foreach (BaseEntity baseEntity in warband) {
            if (baseEntity != null) {
                if (baseEntity.ability == BaseEntity.Ability.TURNEND) {
                    baseEntity.activateAbility();
                }
            }
        }
        OnUpdateWarband?.Invoke();
    }

    /// <summary>
    /// Saves relevant data from the current warband to reproduce in Battle Phase and next Preparation Phase
    /// Also saves these data into the enemy database for retrieval as potential battle phase enemy
    /// Only done in Preparation Phase "End Turn"
    /// </summary>
    private void TransferWarbandInfo() {
        // LOGIC: In Battle Phase: Initialize from EntitiesDatabase based on animalID and change stats
        warbandData.warbandEntities = new List<WarbandDataSO.EntityData>();
        for (int i = 0; i < 5; i++) {
            BaseEntity baseEntity = Instance.warband[i];
            if (baseEntity != null) {
                int animalID = baseEntity.GetAnimalID();
                int attack = baseEntity.GetAttackMax();
                int health = baseEntity.GetHealthMax();
                int position = i;
                int totalEntityCount = baseEntity.totalEntityCount;
                // Create new instance of WarbandDataSO EntityData using the current animal in warband
                WarbandDataSO.EntityData currentAnimal = new WarbandDataSO.EntityData(animalID, attack, health, position, totalEntityCount);
                // Add this instance into the WarbandDataSO warbandEntities
                warbandData.warbandEntities.Add(currentAnimal);
            }
        }
        
        // If doing test runs (e.g. Changing PlayerData.Money to > 10), set isTesting to True (or else our database is inaccurate)
        if (!isTesting) {
            int currentTurn = PlayerData.Instance.TurnNumber;

            // Create new instance of EnemyDatabaseSO TeamData using the current warband's data
            EnemyDatabaseSO.TeamData currentTeam = new EnemyDatabaseSO.TeamData(warbandData.warbandEntities, currentTurn);
            // Add this instance into the EnemyDatabaseSO pastTeams
            enemyDatabase.pastTeams.Add(currentTeam);

            // Other Way --> Using the Dictionary in EnemyDatabaseSO (Incomplete)
            //List<List<WarbandDataSO.EntityData>> pastWarbandDatabase = enemyDatabase.teamDatabase[currentTurn]; 
            //enemyDatabase.teamDatabase[currentTurn].Add(warbandData.warbandEntities);
        }
    }

    /// <summary>
    /// Saves relevant data from frozen animals in the current shop to reproduce in the next Preparation Phase
    /// Only done in Preparation Phase "End Turn"
    /// </summary>
    private void TransferFrozenShopInfo() {
        shopData.frozenShopEntities = new List<ShopDataSO.EntityData>();
        // Iterate through all cards
        for (int i = 0; i < allCards.Count; i++) {
            // If card is enabled (active)
            if(allCards[i].CanGenerate()) {
                // If animal in slot
                if(allCards[i].transform.parent.transform.childCount > 1) {
                    // if animal is frozen
                    if(allCards[i].transform.parent.transform.GetChild(0).gameObject.GetComponent<BaseEntity>().isFrozen) {
                        BaseEntity currentShopAnimal = allCards[i].transform.parent.transform.GetChild(0).gameObject.GetComponent<BaseEntity>();
                        int animalID = currentShopAnimal.GetAnimalID();
                        int position = i;
                        // Create new instance of ShopDataSO EntityData using the current animal in shop
                        ShopDataSO.EntityData currentAnimal = new ShopDataSO.EntityData(animalID, position);
                        // Add this instance into the ShopDataSO frozenShopEntities
                        shopData.frozenShopEntities.Add(currentAnimal);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Counts the number of animals in the warband AFTER any action is carried out (before OnUpdateWarband is invoked);
    /// </summary>
    public int CountWarbandAnimals() {
        int counter = 0;
        foreach (BaseEntity animal in warband) {
            if (animal != null) {
                counter++;
            }
        }
        return counter;
    }
    
    /// <summary>
    /// Control the number of shop slots available
    /// </summary>
    public void ActivateShopSlots() {
        // Debug.Log("Enter PrepManager ActivateShopSlots()");
        int turnNumber = PlayerData.Instance.TurnNumber;
        if (turnNumber == 1) {
            // 3 slots
            SetActiveSpecifiedSlots(3);
        } else if (turnNumber == 2) {
            // 4 slots
            SetActiveSpecifiedSlots(4);
        } else {
            // 5 slots
            SetActiveSpecifiedSlots(5);
        }
    }

    private void SetActiveSpecifiedSlots(int limit) {
        for (int i = 0; i < limit; i++) {
            allCards[i].EnableCard();
        }
    }

    public void ChangeState(CurrentState newState) {
        currentState = newState;
        switch (newState) {
            case CurrentState.TURNSTART:
                RestoreWarbandAnimals();
                StartTurn();
                break;
            case CurrentState.PREPARE:
                break;
            case CurrentState.TURNEND:
                EndTurn();
                TransferFrozenShopInfo();
                TransferWarbandInfo();
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    
    public enum CurrentState { 
        TURNSTART,
        PREPARE,
        TURNEND
    }
}
