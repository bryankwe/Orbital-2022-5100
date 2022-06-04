using UnityEngine;
using UnityEngine.UI;

public class Debug_HealButton : MonoBehaviour {
    /// <summary>
    /// Heal the StatsTracker on Click
    /// </summary>

    [SerializeField] private GameObject getStatsTrackerGameObject;

    private void Start() {
        StatsTracker.TryGetStatsTracker(getStatsTrackerGameObject, out StatsTracker statsTracker, true);

        GetComponent<Button>().onClick.AddListener(() => {
            statsTracker.Heal(10);
        });
    }
}