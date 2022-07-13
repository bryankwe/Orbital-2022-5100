using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// OLD version (For UI Draggable)
// Reference tutorial: Code Monkey (https://www.youtube.com/watch?v=BGr-7GZJNXg&t=292s) & Seif (https://www.youtube.com/watch?v=KL7ACZiKZpg)
public class ItemSlot : MonoBehaviour, IDropHandler
{
    
    // Following Slots Available:
    //    - WARBAND: UIDraggable.Slot.BOTH => Deduct Money / Combine / Swap Position
    //    - FREEZE: UIDraggable.Slot.SHOP => Return to original position in Shop & Freeze
    //    - SELL: UIDraggable.Slot.WARBAND => Destroy & Give $$
    public DragDrop.Slot typeOfItem = DragDrop.Slot.BOTH; // Change this in Unity
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        DragDrop d = eventData.pointerDrag.GetComponent<DragDrop>();

        if(d != null) {
            if(typeOfItem == d.typeOfItem || typeOfItem == DragDrop.Slot.BOTH) {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                d.originalPos = d.transform.position;
            }
        }
    }

}
