using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IGetStatsTracker {

    private StatsTracker statsTracker;

    private void Awake() {
        statsTracker = new StatsTracker(99,99);
        statsTracker.OnDead += StatsTracker_OnDead;
    }

    private void StatsTracker_OnDead(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }

    public void Damage() {
        statsTracker.Damage(60);
    }

    public StatsTracker GetStatsTracker() {
        return statsTracker;
    }


}
