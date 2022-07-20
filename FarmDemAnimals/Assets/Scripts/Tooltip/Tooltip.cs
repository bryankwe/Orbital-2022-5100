using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour {
    
    [SerializeField] private RectTransform canvasRectTransform;
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement; // Disabled if beyond characterWrapLimit
    public int characterWrapLimit;
    public RectTransform rectTransform;
    
    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false); // Deactivate tooltip on awake
    }
    
    /// <summary>
    /// Sets input texts correctly and display in the UI
    /// </summary>
    public void SetText(string content, string header = "") {
        
        headerField.text = header;
        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true: false;
    }

    private void Update() {
        if (Application.isEditor) {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true: false;
        }

        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;

        /*Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        if (anchoredPosition.x + rectTransform.rect.width > canvasRectTransform.rect.width) {
            anchoredPosition.x = canvasRectTransform.rect.width - rectTransform.rect.width;
        }
        if (anchoredPosition.y + rectTransform.rect.height > canvasRectTransform.rect.height) {
            anchoredPosition.y = canvasRectTransform.rect.height - rectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;*/

    }
}
