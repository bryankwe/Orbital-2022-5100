using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationManager : Manager<PreparationManager> {
    
    public EntitiesDatabaseSO entitiesDatabase;
    public List<UICard> allCards; // Contains empty GameObjects used for Instantiation (Assigned in Editor)
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
        OnUpdateWarband += UpdateWarband;
        UpdateWarband();
        ChangeState(CurrentState.TURNSTART);
    }

    /// <summary>
    /// Update the animals in the warband
    /// </summary>
    private void UpdateWarband() {
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
        
        /*foreach (BaseEntity baseEntity in warband) { // DEBUG purpose only
            if (baseEntity != null) {
                Debug.Log(baseEntity.name + " ");
            } else {
                Debug.Log("null ");
            }
        }*/
    }

    /// <summary>
    /// Activate any START OF TURN special abilities
    /// </summary>
    private void StartTurn() {
        foreach (BaseEntity baseEntity in warband) {
            if (baseEntity != null) {
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

    private void TransferWarbandInfo() {
        // !! MUST CHANGE, only pointing, doesn't work
        // Possible ways to fix:
        //     (i) Deep Copy
        //     (ii) Move warband from PreparationManager to GameManager
        GameManager.Instance.playerWarband = Instance.warband; // Simply pointing, not deep copying! (not sure if required)
    }    
    /*public void InvokeOnBuyEvent(BaseEntity animal) {
        OnBuy += animal.activateAbility;
        OnBuy?.Invoke();
        OnBuy -= animal.activateAbility;
    }*/

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
                StartTurn();
                break;
            case CurrentState.PREPARE:
                break;
            case CurrentState.TURNEND:
                EndTurn();
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
