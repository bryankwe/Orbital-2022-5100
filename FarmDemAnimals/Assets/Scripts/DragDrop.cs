using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ALTERNATE version (For UI Draggable)
// Reference tutorial: Code Monkey (https://www.youtube.com/watch?v=BGr-7GZJNXg&t=292s) & Seif (https://www.youtube.com/watch?v=KL7ACZiKZpg)
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Shop items can be dragged to:
    //     - WARBAND slot [buying / combining] (if empty slots available / if same ID)
    //     - FREEZE slot [freezing]
    // Warband items can be dragged to:
    //     - SELL [selling]
    //     - WARBAND [swapping / combining] ( / if same ID)
    public enum Slot { WARBAND, SHOP, BOTH};
    public Slot typeOfItem = Slot.WARBAND; // Change this in Unity

    public Vector2 originalPos;
    
    //[SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");

        originalPos = this.transform.position;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        
        this.transform.position = originalPos;
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

}
