using UnityEngine;

/// <summary>
/// Adds a StatsTracker to a Game Object
/// </summary>
public class StatsTrackerComponent : MonoBehaviour, IGetStatsTracker {

    [Tooltip("Maximum Health amount")]
    [SerializeField] private int healthAmountMax = 99;
    
    [Tooltip("Maximum Attack amount")]
    [SerializeField] private int attackAmountMax = 99;

    [Tooltip("Starting Health amount, leave at 0 to start at full health.")]
    [SerializeField] private int startingHealthAmount;

    [Tooltip("Starting Attack amount, leave at 0 to start at full attack.")]
    [SerializeField] private int startingAttackAmount;

    private StatsTracker statsTracker;


    private void Awake() {
        // Create StatsTracker
        statsTracker = new StatsTracker(healthAmountMax, attackAmountMax);

        if (startingHealthAmount != 0) {
            statsTracker.SetHealth(startingHealthAmount);
        }

        if (startingAttackAmount != 0) {
            statsTracker.SetAttack(startingAttackAmount);
        }
    }

    /// <summary>
    /// Get the StatsTracker created by this Component
    /// </summary>
    public StatsTracker GetStatsTracker() {
        return statsTracker;
    }


}
