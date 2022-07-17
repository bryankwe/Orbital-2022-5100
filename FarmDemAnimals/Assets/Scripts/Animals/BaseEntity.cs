using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IGetStatsTracker {

    // Death ability: OnDead
    // Enrage ability: OnDamaged && !isDead
    // Kill: target OnDead && !isDead
    public enum Ability { BUY, SELL, TURNSTART, TURNEND, COMBINE, DEATH, ENRAGE, KILL };
    public abstract Ability ability { get; }// public abstract string ability { get; }
    public UIShop shopRef = null; // To be changed when instantiated in the shop
    public BattleManager battleRef = null; // To be changed when instantiated in the battle phase
    public bool isFrozen = false;
    public int totalEntityCount = 1; // Number of times the entity has been combined
    public enum Team { PLAYER, ENEMY };
    public Team team = Team.PLAYER; // To be changed in the battle phase
    public BaseEntity target = null; // BATTLE PHASE -> Set to opponent

    private protected StatsTracker statsTracker;
    [SerializeField] private protected int initialHealth;
    [SerializeField] private protected int initialAttack;
    [SerializeField] private protected int animalID;
    [SerializeField] private protected string animalName;
    [SerializeField] private protected int animalTier;

    public ParticleSystem PowerUpParticleSystem;
    public ParticleSystem DamagedParticleSystem;

    public abstract void activateAbility();

    private void Awake() {
        statsTracker = new StatsTracker(initialHealth, initialAttack);
        statsTracker.OnDead += StatsTracker_OnDead;
        statsTracker.OnStatsIncreased += StatsTracker_OnStatsIncreased;
        statsTracker.OnDamaged += StatsTracker_OnDamaged;
    }

    private void OnDisable() {
        statsTracker.OnDead -= StatsTracker_OnDead;
        statsTracker.OnHealthChanged += StatsTracker_OnStatsIncreased;
        statsTracker.OnDamaged -= StatsTracker_OnDamaged;
    }

    private void StatsTracker_OnDead(object sender, System.EventArgs e) {
        ActivateAbilityBeforeDeath();
        Destroy(gameObject);
    }

    private void StatsTracker_OnStatsIncreased(object sender, System.EventArgs e) {
        PowerUpParticleSystem.transform.GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>().StartParticleEmission();
        SoundManager.Instance.Play("PowerUp");
    }

    private void StatsTracker_OnDamaged(object sender, System.EventArgs e) {
        DamagedParticleSystem.transform.GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>().StartParticleEmission();
        ActivateAbilityAfterDamaged();
    }

    public void FreezeToggle() {
        transform.Find("FreezeBG").gameObject.SetActive(!isFrozen);
        isFrozen = !isFrozen;
        SoundManager.Instance.Play("Freeze");
        //Debug.Log(transform.name + " Freeze Status: " + isFrozen.ToString());
    }

    // -------------------------- PREPARATION FUNCTIONS -------------------------------
    
    /// <summary>
    /// PREPARATION PHASE
    /// Get current Health Max
    /// </summary>
    public int GetHealthMax() {
        return statsTracker.GetHealthMax();
    }

    /// <summary>
    /// PREPARATION PHASE
    /// Get current Attack
    /// </summary>
    public int GetAttackMax() {
        return statsTracker.GetAttackMax();
    }

    /// <summary>
    /// PREPARATION PHASE
    /// Animal increase health and damage by input amounts
    /// Override if special ability in Preparation Phase
    /// </summary>
    /// <param name="healthAmount">The health amount to increase by</param>
    /// <param name="damageAmount">The damage amount to increase by</param>
    public virtual void IncreasePreparationStats(int damageAmount, int healthAmount) {
        statsTracker.IncreaseAttackMax(damageAmount);
        statsTracker.IncreaseHealthMax(healthAmount);
    }

    // -------------------------- TRANSITIONAL FUNCTIONS -------------------------------
    
    /*/// <summary>
    /// MIGHT NOT USE (see SetStats())
    /// BEFORE PREPARATION PHASE, AFTER BATTLE PHASE
    /// Resets to maximum health and attack amount
    /// </summary>
    public virtual void ResetStats() {
        statsTracker.ResetHealth();
        statsTracker.ResetAttack();
    }*/

    /// <summary>
    /// USE IN PLACE OF ResetStats()
    /// Before PREPARATION PHASE
    /// Before BATTLE PHASE
    /// Resets to maximum health and attack amount
    /// </summary>
    /// <param name="healthAmount">The health amount to reset to</param>
    /// <param name="damageAmount">The damage amount to reset to</param>
    public virtual void SetStats(int damageAmount, int healthAmount) {
        statsTracker.SetAttackMax(damageAmount,true);
        statsTracker.SetHealthMax(healthAmount,true);
    }

    // -------------------------- BATTLE FUNCTIONS -------------------------------

    /// <summary>
    /// BATTLE PHASE
    /// Get current Health
    /// </summary>
    public int GetHealth() {
        return statsTracker.GetHealth();
    }

    /// <summary>
    /// BATTLE PHASE
    /// Get current Health
    /// </summary>
    public int GetAttack() {
        return statsTracker.GetAttack();
    }

    /// <summary>
    /// BATTLE PHASE
    /// Used ONLY by on OWN team: Bee (Death), BlueBird (Death), Duck (Enrage), Rhino (Kill), FatBird (Death)
    /// Called on OWN team's Warband
    /// Animal increase health and damage by input amounts
    /// Override if special ability in Battle Phase
    /// </summary>
    /// <param name="healthAmount">The health amount to increase by</param>
    /// <param name="damageAmount">The damage amount to increase by</param>
    public virtual void IncreaseBattleStats(int damageAmount, int healthAmount) {
        statsTracker.Buff(damageAmount);
        statsTracker.Heal(healthAmount);
    }

    /// <summary>
    /// BATTLE PHASE
    /// Used by all animals on their target enemy
    /// Called on OPPONENT team's Warband when fighting
    /// Animal decrease health and damage by input amounts
    /// </summary>
    /// <param name="healthAmount">The health amount to decrease by</param>
    /// <param name="damageAmount">The damage amount to decrease by</param>
    public virtual void DecreaseBattleStats(int damageAmount, int healthAmount) {
        statsTracker.Nerf(damageAmount);
        statsTracker.Damage(healthAmount);
    }

    /// <summary>
    /// BATTLE PHASE
    /// Check whether animal is dead
    /// </summary>
    public virtual bool IsDead() {
        return statsTracker.IsDead();
    }

    /// <summary>
    /// BATTLE PHASE
    /// Invoke the OnDead event (do any special ability before dying and kill)
    /// </summary>
    public virtual void Die() {
        Debug.Log("To Die: " + gameObject.name);
        statsTracker.Die();
    }

    /// <summary>
    /// BATTLE PHASE
    /// Used ONLY (and Overriden) by ENRAGE ability: Duck
    /// This is a placeholder function meant to be overriden
    /// </summary>
    public virtual void ActivateAbilityAfterDamaged() {
        
    }
    
    /// <summary>
    /// BATTLE PHASE
    /// Used by all
    /// Overriden by DEATH ability: Bee, BlueBird, FatBird
    /// This is a placeholder function meant to be overriden
    /// </summary>
    public virtual void ActivateAbilityBeforeDeath() {
        if (target.ability == BaseEntity.Ability.KILL) {
            target.ActivateKillAbility();
        }
    }

    /// <summary>
    /// BATTLE PHASE
    /// Used ONLY (and Overriden) by KILL ability: Rhino
    /// This is a placeholder function meant to be overriden
    /// </summary>
    public virtual void ActivateKillAbility() {

    }
    
    // -------------------------- MISCELLANEOUS FUNCTIONS -------------------------------
    
    public StatsTracker GetStatsTracker() {
        return statsTracker;
    }

    public int GetAnimalID() {
        return animalID;
    }

    public int GetAnimalTier() {
        return animalTier;
    }
}
