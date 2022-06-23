using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IGetStatsTracker {

    public UIShop shopRef = null;
    public bool isFrozen = false;
    public int combineCount = 0; // Number of times the entity has been combined

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

    public void FreezeToggle() {
        transform.Find("FreezeBG").gameObject.SetActive(!isFrozen);
        isFrozen = !isFrozen;
        //Debug.Log(transform.name + " Freeze Status: " + isFrozen.ToString());
    }

    /*public int GetHealth() {
        statsTracker.GetHealth();
    }

    public int GetAttack() {
        statsTracker.GetAttack();
    }*/

    /// <summary>
    /// Animal takes input amount of damage (decrease health)
    /// Override if special ability upon Damage
    /// </summary>
    public virtual void Damage(int amount) {
        statsTracker.Damage(amount);
    }

    /// <summary>
    /// Animal heals by input amount (increase health)
    /// Override if special ability upon Heal
    /// </summary>
    public virtual void Heal(int amount) {
        statsTracker.Heal(amount);
    }
    
    public StatsTracker GetStatsTracker() {
        return statsTracker;
    }

    public int GetAnimalID() {
        return animalID;
    }

}