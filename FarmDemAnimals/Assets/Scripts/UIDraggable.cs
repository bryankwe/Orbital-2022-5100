using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: quill18creates (https://www.youtube.com/watch?v=AM7wBz9azyU)

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Transform parentToReturnTo = null;

    // Shop items can be dragged to WARBAND & FREEZE
    // Warband items can be dragged to SELL (& SWAPPED around WARBAND)
    public enum Slot { WARBAND, SHOP, SELL, FREEZE};
    public Slot typeOfItem = Slot.WARBAND; // Change this in Unity

    //[SerializeField] private Canvas canvas;
    //private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        //rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");

        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        canvasGroup.blocksRaycasts = false;

        //TODO: Make Droppable Slots glow?
        /*UIDroppable[] slots = GameObject.FindObjectsOfType<UIDroppable>();
        for (int i = 0; i < slots.Length; i++) {

        }*/
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        this.transform.position = eventData.position;
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        canvasGroup.blocksRaycasts = true;
    }

}