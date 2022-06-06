using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IGetStatsTracker {

    private protected StatsTracker statsTracker;
    [SerializeField] private protected int initialHealth;
    [SerializeField] private protected int initialAttack;
    [SerializeField] private protected int animalID;
    [SerializeField] private protected string animalName;
    [SerializeField] private protected int animalTier;

    private void Awake() {
        statsTracker = new StatsTracker(initialHealth, initialAttack);
        statsTracker.OnDead += StatsTracker_OnDead;
    }

    private void StatsTracker_OnDead(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }

    public abstract void Damage();

    public StatsTracker GetStatsTracker() {
        return statsTracker;
    }

    /*private void Awake() {
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
    }*/

}
