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

    public int GetHealth() {
        return statsTracker.GetHealth();
    }

    public int GetAttack() {
        return statsTracker.GetAttack();
    }

    /// <summary>
    /// PREPARATION PHASE
    /// Animal increase health and damage by input amounts
    /// Override if special ability in Preparation Phase
    /// </summary>
    /// <param name="healthAmount">The health amount to increase by</param>
    /// <param name="damageAmount">The damage amount to increase by</param>
    public virtual void IncreasePreparationStats(int healthAmount, int damageAmount) {
        statsTracker.IncreaseHealthMax(healthAmount);
        statsTracker.IncreaseAttackMax(damageAmount);
    }

    /// <summary>
    /// BATTLE PHASE
    /// Animal takes input amount of damage (decrease health)
    /// Override if special ability upon Damage
    /// </summary>
    public virtual void Damage(int amount) {
        statsTracker.Damage(amount);
    }

    /// <summary>
    /// BATTLE PHASE
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
