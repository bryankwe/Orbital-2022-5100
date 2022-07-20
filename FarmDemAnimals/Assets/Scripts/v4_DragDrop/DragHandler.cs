using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: Kiwasi Games (https://www.youtube.com/watch?v=c47QYgsJrWc&t=11s)
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // Shop items (Origin) can be dragged to:
    //     - WARBAND slot [buying / combining] (if empty slots available / if same ID)
    //     - FREEZE slot [freezing]
    // Warband items (Origin) can be dragged to:
    //     - SELL slot [selling]
    //     - WARBAND slot [swapping / combining] ( / if same ID)
    
    public enum Origin { WARBAND, SHOP, BOTH};
    
    // Always starts off as SHOP because instantiated in SHOP. MODIFY during runtime
    public Origin typeOfItem = Origin.SHOP;
    
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent; //Updated only OnBeginDrag
    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Starts the movement of the dragged object
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData) {
        //Debug.Log("OnBeginDrag");
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        SoundManager.Instance.Play("Drag");
    }

    /// <summary>
    /// Moves the dragged object's position accordingly to the mouse position
    /// </summary>
    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        this.transform.position = eventData.position;
    }

    /// <summary>
    /// Resets the dragged object's position to the origginal position if the parent did not change
    /// </summary>
    public void OnEndDrag(PointerEventData eventData) {
        //Debug.Log("OnEndDrag");
        itemBeingDragged = null;
        canvasGroup.blocksRaycasts = true;
        if(transform.parent == startParent) {
            transform.position = startPosition;
        }
    }
}
