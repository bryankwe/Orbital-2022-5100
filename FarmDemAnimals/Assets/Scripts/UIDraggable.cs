using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: quill18creates (https://www.youtube.com/watch?v=AM7wBz9azyU)

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Transform parentToReturnTo = null;

    // Shop items can be dragged to:
    //     - WARBAND slot [buying / combining] (if empty slots available / if same ID)
    //     - FREEZE slot [freezing]
    // Warband items can be dragged to:
    //     - SELL [selling]
    //     - WARBAND [swapping / combining] ( / if same ID)
    public enum Slot { WARBAND, SHOP, BOTH};
    public Slot typeOfItem = Slot.WARBAND; // Change this in Unity

    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");

        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        canvasGroup.blocksRaycasts = false;

        //TODO: Make Droppable Slots glow?
        //UIDroppable[] slots = GameObject.FindObjectsOfType<UIDroppable>();
        //for (int i = 0; i < slots.Length; i++) {
        //
        //}
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        canvasGroup.blocksRaycasts = true;

        //For combinables? dk...
        //EventSystem.current.RaycastAll(eventData,);
    }

}