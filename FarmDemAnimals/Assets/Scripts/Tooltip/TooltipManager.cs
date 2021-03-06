using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : Manager<TooltipManager> {
    public Tooltip tooltip;

    /// <summary>
    /// Shows the Tooltip upon call
    /// </summary>
    public static void Show(string content, string header = "") {
        Instance.tooltip.SetText(content, header);
        Instance.tooltip.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the Tooltip upon call
    /// </summary>
    public static void Hide() {
        Instance.tooltip.gameObject.SetActive(false);
    }
}