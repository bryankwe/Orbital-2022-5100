using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: quill18creates (https://www.youtube.com/watch?v=AM7wBz9azyU)
public class UIDroppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    
    public UIDraggable.Slot typeOfItem = UIDraggable.Slot.SHOP; // Change this in Unity
    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData) {
        //Debug.Log("OnPointerExit");
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        UIDraggable d = eventData.pointerDrag.GetComponent<UIDraggable>();

        if(d != null) {
            if(typeOfItem == d.typeOfItem) {// || typeOfItem == UIDraggable.Slot.INVENTORY) {
                d.parentToReturnTo = this.transform;
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }
}
