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
    //public DragHandler.Origin typeOfItem = DragHandler.Origin.BOTH; // Change this in Unity

    // Returns the GameObject that is in the Slot, if available
    public GameObject item {
        get {
            if(transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        if(!item) {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }

        //UIDraggable d = eventData.pointerDrag.GetComponent<UIDraggable>();

        /*if(d != null) {
            if(typeOfItem == d.typeOfItem) {// || typeOfItem == UIDraggable.Slot.INVENTORY) {
                d.parentToReturnTo = this.transform;
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            }
        }*/
    }
}
