using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationManager : Manager<PreparationManager> {
    
    public EntitiesDatabaseSO entitiesDatabase;
    public List<UICard> allCards; // Contains empty GameObjects used for Instantiation (Assigned in Editor)
    public List<Slot> warbandSlots; // Contains Slots to retrieve Animals in Warband (Assigned in Editor)
    public List<BaseEntity> warband = new List<BaseEntity>(); // To be updated upon click of "End Turn" Button?

    public System.Action OnUpdateWarband; // Buy, Combine, Swap Positions, Sell
    
    private void Start() {
        OnUpdateWarband += UpdateWarband;
    }

    private void UpdateWarband() {
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

    
    /*
    NOTE: JUST COPYING CODE FROM TUTORIAL (NEED TO THINK HOW TO ADAPT TO OUR GAME)
    //OLD VERSION (FOR non-UI draggable)
    //Reference Tutorial: TaroDev (https://www.youtube.com/watch?v=Tv82HIvKcZQ) & (https://www.youtube.com/watch?v=o_qEXZhQR-M)

    [SerializeField] private List<DropSlot> slots;
    [SerializeField] private Draggable draggable;
    [SerializeField] private Transform slotParent, draggableParent;
    
    void Spawn() {
        var randomSet = slots.OrderBy(s => Random.value).Take(3).ToList();

        for (int i = 0; i < randomSet.Count; i++) {
            var spawnedSlot = Instantiate(randomSet[i], slotParent.GetChild(i).position, Quaternion.identity);

            var spawnedDraggable = Instantiate(draggable, draggableParent.GetChild(i).position, Quaternion.identity);
        }
    }*/
}
