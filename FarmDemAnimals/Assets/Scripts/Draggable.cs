using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OLD VERSION (FOR non-UI draggable)
//Reference Tutorial: TaroDev (https://www.youtube.com/watch?v=Tv82HIvKcZQ) & (https://www.youtube.com/watch?v=o_qEXZhQR-M)
//Other Tutorials: GameDevel (https://www.youtube.com/watch?v=uk_E_cGrlQc)
public class Draggable : MonoBehaviour {
    
    private bool dragging, placed;
    private Vector2 offset, originalPosition;
    private Camera cam;
    private float speed = 10;
    private DropSlot slot;

    void Awake() {
        cam = Camera.main;
        originalPosition = transform.position;
    }

    void Update() {
        if (placed) return;
        if (!dragging) return;

        var mousePosition = GetMousePos();

        transform.position = mousePosition - offset;
    }

    void OnMouseDown() {
        dragging = true;
        offset = GetMousePos() - (Vector2)transform.position;
    }

    /*void OnMouseDrag() {
        transform.position = Vector2.MoveTowards(transform.position, GetMousePos() + dragOffset, speed * Time.deltaTime);
    }*/

    void OnMouseUp() {
        if (Vector2.Distance(transform.position, slot.transform.position) < 3) {
            transform.position = slot.transform.position;
            slot.Placed();
            placed = true;   
        } else {
            transform.position = originalPosition;
            dragging = false;
        }
    }

    Vector2 GetMousePos() {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}

