using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI Stats Tracker, sets the stats based on the linked StatsTracker
/// </summary>
public class StatsTrackerUI : MonoBehaviour {
    
    [Tooltip("Optional; Either assign a reference in the Editor (that implements IGetStatsTracker) or manually call SetStatsTracker()")]
    [SerializeField] private GameObject getStatsTrackerGameObject;

    [Tooltip("Image to show the Health Bar, should be set as Fill, the script modifies fillAmount")]
    [SerializeField] private Image image;


    private StatsTracker statsTracker;


    private void Start() {
        if (StatsTracker.TryGetStatsTracker(getStatsTrackerGameObject, out StatsTracker statsTracker)) {
            SetStatsTracker(statsTracker);
        }
    }

    /// <summary>
    /// Set the Health System for this Health Bar
    /// </summary>
    public void SetStatsTracker(StatsTracker statsTracker) {
        if (this.statsTracker != null) {
            this.statsTracker.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
        this.statsTracker = statsTracker;

        UpdateHealthBar();

        statsTracker.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    /// <summary>
    /// Event fired from the Health System when Health Amount changes, update Health Bar
    /// </summary>
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
        UpdateHealthBar();
    }

    /// <summary>
    /// Update Health Bar using the Image fillAmount based on the current Health Amount
    /// </summary>
    private void UpdateHealthBar() {
        //image.fillAmount = healthSystem.GetHealthNormalized();
    }

    /// <summary>
    /// Clean up events when this Game Object is destroyed
    /// </summary>
    private void OnDestroy() {
        statsTracker.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }

}

