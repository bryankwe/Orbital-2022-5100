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

    // Need to include logic for combine => Check if id is same? but I cannot access BaseEntity.id through DragHandler
    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();

        BaseEntity a = eventData.pointerDrag.GetComponent<BaseEntity>();

        /*if (eventData.pointerDrag.TryGetComponent(out BaseEntity animal)) { // Always true
            animal.shopRef.AllowDragToWarband
        }*/

        //if(DragHandler.itemBeingDragged.transform.parent.gameObject.tag == "Shop")

        if(d != null) {

            if(typeOfItem == DragHandler.Origin.BOTH) { // if the slot is WARBAND
                if (d.typeOfItem == DragHandler.Origin.SHOP) { // If dragged from Shop
                    ShopToWarband(d, a);
                } else { // If dragged from Warband (Swap positions)
                    WarbandToWarband(d, a);
                }
            } else if(typeOfItem == DragHandler.Origin.SHOP) { // if the slot is FREEZE
                if(d.typeOfItem == DragHandler.Origin.SHOP) { // If dragged from Shop
                    ShopToFreeze(d, a);
                }
            } else if(typeOfItem == DragHandler.Origin.WARBAND) { // if the slot is SELL
                if(d.typeOfItem == DragHandler.Origin.WARBAND) { // If dragged from Warband
                    WarbandToSell(d, a);
                }
            }
        }
    }

    void ShopToWarband(DragHandler d, BaseEntity a) {
        if (!item && a.shopRef.OnDragToWarband()) { // If enough money to buy animal & the warband slot is empty
            if(a.isFrozen) {
                a.FreezeToggle(); // Unfreeze
            }
            d.typeOfItem = DragHandler.Origin.WARBAND; // Set the Origin to WARBAND
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        } else if (item.transform.GetComponent<BaseEntity>().GetAnimalID() == a.GetAnimalID() && a.shopRef.OnDragToWarband()) { 
            // If enough money to buy animal & Same Unit in Slot (Combine)
            CombineAnimals(a);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        }
    }

    void WarbandToWarband(DragHandler d, BaseEntity a) {
        if (!item) { // If warband slot is empty
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        } else if (item == a.gameObject) { // If unit is dragged onto same slot
            return;
        } else if (item.transform.GetComponent<BaseEntity>().GetAnimalID() == a.GetAnimalID()) { // If Same unit in Slot (Combine)
            CombineAnimals(a);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        } else { // If different unit in Slot (Swap)
            item.transform.SetParent(DragHandler.itemBeingDragged.transform.parent.transform);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            PreparationManager.Instance.OnUpdateWarband?.Invoke();
        }
    }

    void ShopToFreeze(DragHandler d, BaseEntity a) {
        /*if(!a.isFrozen) {
            a.FreezeToggle(); // Freeze
        }*/
        a.FreezeToggle(); // Freeze or Unfreeze
    }

    void WarbandToSell(DragHandler d, BaseEntity a) {
        a.shopRef.SellSuccess(a.totalEntityCount); // Add Money
        Destroy(d.gameObject); // Destroy Animal
        PreparationManager.Instance.OnUpdateWarband?.Invoke();
    }

    void CombineAnimals(BaseEntity a) {
        item.transform.GetComponent<BaseEntity>().IncreasePreparationStats(a.GetHealth(), a.GetAttack());
        item.transform.GetComponent<BaseEntity>().totalEntityCount += 1;
        Destroy(a.gameObject); // Destroy dragged (duplicate) animal
    }
}
