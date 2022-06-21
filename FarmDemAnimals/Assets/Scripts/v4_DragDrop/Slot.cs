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

        if(d != null && !item) {

            /*if(!item) { // Nothing in Slot

            } else if (item.transform.GetComponent<BaseEntity>().GetAnimalID() == a.GetAnimalID()) { // Same Unit in Slot (Combine)

            } else if () { // Different Unit in Slot (Swap)

            }*/

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
                //destroy the animal
                //add money => How to access UIShop via this script?
            }
        }
    }

    void ShopToWarband(DragHandler d, BaseEntity a) {
        if (a.shopRef.OnDragToWarband()) { // If enough money to buy animal
            if(a.isFrozen) {
                a.FreezeToggle(); // Unfreeze
            }
            d.typeOfItem = DragHandler.Origin.WARBAND; // Set the Origin to WARBAND
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
    }

    void WarbandToWarband(DragHandler d, BaseEntity a) {
        DragHandler.itemBeingDragged.transform.SetParent(transform);
    }

    void ShopToFreeze(DragHandler d, BaseEntity a) {
        if(!a.isFrozen) {
            a.FreezeToggle(); // Freeze
        }
    }

    void WarbandToSell(DragHandler d, BaseEntity a) {
        a.shopRef.SellSuccess();
        Destroy(d.gameObject);
    }
}
