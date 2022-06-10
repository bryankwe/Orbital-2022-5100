using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Transform parentToReturnTo = null;

    public enum Slot { WARBAND, SHOP, SELL, FREEZE};
    public Slot typeOfItem = Slot.WARBAND; // Change this in Unity

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");

        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //TODO: Make Droppable Slots glow?
        /*UIDroppable[] slots = GameObject.FindObjectsOfType<UIDroppable>();
        for (int i = 0; i < slots.Length; i++) {

        }*/
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}