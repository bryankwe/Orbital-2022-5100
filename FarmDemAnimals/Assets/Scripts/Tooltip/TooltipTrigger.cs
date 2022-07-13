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
    
    public void OnPointerEnter(PointerEventData eventData) {
        delay = LeanTween.delayedCall(0.35f, () => {
            TooltipManager.Show(content, header);
         });
    }

    public void OnPointerClick(PointerEventData eventData) {
        TooltipManager.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData) {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.Hide();
    }
}