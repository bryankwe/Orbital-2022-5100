using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: Kiwasi Games (https://www.youtube.com/watch?v=c47QYgsJrWc&t=11s)
public class Slot : MonoBehaviour, IDropHandler {
    
    // Following Slots Available:
    //    - WARBAND: DragHandler.Origin.BOTH => Deduct Money / Combine / Swap Position
    //    - FREEZE: DragHandler.Origin.SHOP => Return to original position in Shop & Freeze
    //    - SELL: DragHandler.Origin.WARBAND => Destroy & Give $$
    
    // Change this in Unity. NO need to modify during RUNTIME
    // Note: Set this to the allowed origin of the draggable
    public DragHandler.Origin typeOfItem = DragHandler.Origin.BOTH;

    // Returns the GameObject that is in the Slot, if available
    public GameObject item {
        get {
            if(transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    /// <summary>
    /// Drops the DragHandler object on the Slot if allowed
    /// Calls many helper functions for conditional checks in the various allowable ways to be dropped
    /// </summary>
    public void OnDrop(PointerEventData eventData) {
        //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();

        BaseEntity a = eventData.pointerDrag.GetComponent<BaseEntity>();

        if(d != null) {

            if(typeOfItem == DragHandler.Origin.BOTH) { // if the slot is WARBAND
                if (d.typeOfItem == DragHandler.Origin.SHOP) { // If dragged from Shop
                    ShopToWarband(d, a);
                    SoundManager.Instance.Play("Drop");
                } else { // If dragged from Warband (Swap positions)
                    WarbandToWarband(d, a);
                    SoundManager.Instance.Play("Drop");
                }
            } else if(typeOfItem == DragHandler.Origin.SHOP) { // if the slot is FREEZE
                if(d.typeOfItem == DragHandler.Origin.SHOP) { // If dragged from Shop
                    ShopToFreeze(d, a);
                    SoundManager.Instance.Play("Drop");
                } else { // If dragged from elsewhere (which is not allowed)
                    SoundManager.Instance.Play("Error");
                }
            } else if(typeOfItem == DragHandler.Origin.WARBAND) { // if the slot is SELL
                if(d.typeOfItem == DragHandler.Origin.WARBAND) { // If dragged from Warband
                    WarbandToSell(d, a);
                    SoundManager.Instance.Play("Drop");
                } else { // If dragged from elsewhere (which is not allowed)
                    SoundManager.Instance.Play("Error");
                }
            }
        }
    }

    /// <summary>
    /// Helper function for OnDrop()
    /// Checks and plays logic for various ways animal can be dragged from Shop and dropped in Warband
    /// </summary>
    void ShopToWarband(DragHandler d, BaseEntity a) {
        if (!item && a.shopRef.OnDragToWarband()) { // If enough money to buy animal & the warband slot is empty
            if(a.isFrozen) {
                a.FreezeToggle(); // Unfreeze
            }
            d.typeOfItem = DragHandler.Origin.WARBAND; // Set the Origin to WARBAND
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
            if(a.ability == BaseEntity.Ability.BUY) { // Activate animal's BUY special ability
                a.activateAbility();
                //PreparationManager.Instance.OnBuy?.Invoke();
            }
        } else if (item.transform.GetComponent<BaseEntity>().GetAnimalID() == a.GetAnimalID() && a.shopRef.OnDragToWarband()) { 
            // If enough money to buy animal & Same Unit in Slot (Combine)
            CombineAnimals(a, true);
        }
    }

    /// <summary>
    /// Helper function for OnDrop()
    /// Checks and plays logic for various ways animal can be dragged from Warband and dropped in Warband
    /// </summary>
    void WarbandToWarband(DragHandler d, BaseEntity a) {
        if (!item) { // If warband slot is empty
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        } else if (item == a.gameObject) { // If unit is dragged onto same slot
            return;
        } else if (item.transform.GetComponent<BaseEntity>().GetAnimalID() == a.GetAnimalID()) { // If Same unit in Slot (Combine)
            CombineAnimals(a, false);
        } else { // If different unit in Slot (Swap)
            item.transform.SetParent(DragHandler.itemBeingDragged.transform.parent.transform);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        }
    }

    /// <summary>
    /// Helper function for OnDrop()
    /// Checks and plays logic for various ways animal can be dragged from Shop and dropped in Freeze
    /// </summary>
    void ShopToFreeze(DragHandler d, BaseEntity a) {
        /*if(!a.isFrozen) {
            a.FreezeToggle(); // Freeze
        }*/
        a.FreezeToggle(); // Freeze or Unfreeze
    }

    /// <summary>
    /// Helper function for OnDrop()
    /// Checks and plays logic for various ways animal can be dragged from Warband and dropped in Sell
    /// </summary>
    void WarbandToSell(DragHandler d, BaseEntity a) {
        a.shopRef.SellSuccess(a.totalEntityCount); // Add Money
        if(a.ability == BaseEntity.Ability.SELL) { // Activate animal's SELL special ability
            a.activateAbility(); // Must ensure the ability does not apply to self (haven't destroy self yet)
            //PreparationManager.Instance.OnSell?.Invoke();
        }
        Destroy(d.gameObject); // Destroy Animal
        PreparationManager.Instance.OnUpdateWarband?.Invoke();
    }

    /// <summary>
    /// Helper function for OnDrop()
    /// Checks and plays logic for various ways animal can be combined with each other
    /// </summary>
    void CombineAnimals(BaseEntity a, bool fromShop) {
        BaseEntity itemBE = item.transform.GetComponent<BaseEntity>();
        itemBE.IncreasePreparationStats(a.GetAttackMax(), a.GetHealthMax());
        itemBE.totalEntityCount += 1;
        Destroy(a.gameObject); // Destroy dragged (duplicate) animal
        PreparationManager.Instance.OnUpdateWarband?.Invoke();
        if(itemBE.ability == BaseEntity.Ability.BUY && fromShop) { // If Combined from Shop and ability is BUY
            itemBE.activateAbility();
            //PreparationManager.Instance.OnBuy?.Invoke();
        } else if (itemBE.ability == BaseEntity.Ability.COMBINE && fromShop) { // If Combined from Shop and ability is COMBINE
            itemBE.activateAbility();
        } else if (itemBE.ability == BaseEntity.Ability.COMBINE && !fromShop) { // If Combined from Warband and ability is COMBINE
            itemBE.activateAbility();
        }
    }
}
