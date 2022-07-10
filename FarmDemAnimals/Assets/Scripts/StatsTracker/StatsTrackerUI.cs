using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Simple UI Stats Tracker, sets the stats based on the linked StatsTracker
/// </summary>
public class StatsTrackerUI : MonoBehaviour {
    
    [Tooltip("Optional; Either assign a reference in the Editor (that implements IGetStatsTracker) or manually call SetStatsTracker()")]
    [SerializeField] private GameObject getStatsTrackerGameObject;

    [Tooltip("TextMesh that displays the attack stats")]
    [SerializeField] private TextMeshProUGUI attackText;

    [Tooltip("TextMesh that displays the health stats")]
    [SerializeField] private TextMeshProUGUI healthText;


    private StatsTracker statsTracker;


    private void Start() {
        if (StatsTracker.TryGetStatsTracker(getStatsTrackerGameObject, out StatsTracker statsTracker)) {
            SetStatsTracker(statsTracker);
        }
    }

    /// <summary>
    /// Set the Health and Attack for this StatsTracker
    /// </summary>
    public void SetStatsTracker(StatsTracker statsTracker) {
        if (this.statsTracker != null) {
            this.statsTracker.OnHealthChanged -= StatsTracker_OnHealthChanged;
            this.statsTracker.OnAttackChanged -= StatsTracker_OnAttackChanged;
        }
        this.statsTracker = statsTracker;

        UpdateHealthComponent();
        UpdateAttackComponent();

        statsTracker.OnHealthChanged += StatsTracker_OnHealthChanged;
        statsTracker.OnAttackChanged += StatsTracker_OnAttackChanged;
    }

    /// <summary>
    /// Event fired from the StatsTracker when Health Amount changes, update Health Stats
    /// </summary>
    private void StatsTracker_OnHealthChanged(object sender, System.EventArgs e) {
        UpdateHealthComponent();
    }

    /// <summary>
    /// Update Health Stats to the current Health Amount
    /// </summary>
    private void UpdateHealthComponent() {
        var child = transform.GetChild(2);
        healthText = child.GetComponentInChildren<TextMeshProUGUI>();
        healthText.text = statsTracker.GetHealth().ToString();
    }

    /// <summary>
    /// Event fired from the StatsTracker when Attack Amount changes, update Attack Stats
    /// </summary>
    private void StatsTracker_OnAttackChanged(object sender, System.EventArgs e) {
        UpdateAttackComponent();
    }

    /// <summary>
    /// Update Attack Stats to the current Attack Amount
    /// </summary>
    private void UpdateAttackComponent() {
        var child = transform.GetChild(3);
        attackText = child.GetComponentInChildren<TextMeshProUGUI>();
        attackText.text = statsTracker.GetAttack().ToString();
    }

    /// <summary>
    /// Clean up events when this Game Object is destroyed
    /// </summary>
    /*private void OnDisable() {
        
        statsTracker.OnHealthChanged -= StatsTracker_OnHealthChanged;
        statsTracker.OnAttackChanged -= StatsTracker_OnAttackChanged;
        Debug.Log("StatsTrackerUI destroyed: " + transform.parent.name);
    }*/

}

