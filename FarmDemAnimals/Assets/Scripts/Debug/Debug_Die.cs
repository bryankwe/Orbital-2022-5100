using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Die : MonoBehaviour {
    /// <summary>
    /// Make the StatsTracker Die on Click
    /// </summary>

    [SerializeField] private GameObject getStatsTrackerGameObject;

    private void Start() {
        StatsTracker.TryGetStatsTracker(getStatsTrackerGameObject, out StatsTracker statsTracker, true);

        GetComponent<Button>().onClick.AddListener(() => {
            getStatsTrackerGameObject.GetComponent<BaseEntity>().Die();
        });
    }
}
