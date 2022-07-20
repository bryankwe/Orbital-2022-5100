using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    
    private static LTDescr delay;
    public string header;
    [Multiline()] public string content;

    private void Awake() {
        //TooltipManager.Hide();
    }
    
    /// <summary>
    /// Shows the Tooltip upon hovering over the element after a delayed time
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData) {
        delay = LeanTween.delayedCall(0.35f, () => {
            TooltipManager.Show(content, header);
         });
    }

    /// <summary>
    /// Shows the Tooltip upon clicking the element
    /// </summary>
    public void OnPointerClick(PointerEventData eventData) {
        TooltipManager.Show(content, header);
    }

    /// <summary>
    /// Hides the Tooltip upon hovering out of the element
    /// </summary>
    public void OnPointerExit(PointerEventData eventData) {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.Hide();
    }
}