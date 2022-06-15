using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Draggable
// Reference Tutorial: Kiwasi Games (https://www.youtube.com/watch?v=c47QYgsJrWc&t=11s)
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        itemBeingDragged = null;
        canvasGroup.blocksRaycasts = true;
        if(transform.parent == startParent) {
            transform.position = startPosition;
        } 
    }
}
