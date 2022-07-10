using System;
using UnityEngine;

public class StatsTracker {
    /// <summary>
    /// Stats Tracker: Health, Attack
    /// Health: Damage, Heal, fires several events when data changes.
    /// Attack: Nerf, Buff, fires several events when data changes.
    /// All Units (Animals) have a StatsTracker
    /// Use HealthSystemComponent if you want to add a HealthSystem directly to a Game Object instead of using the C# constructor
    /// </summary>

    public event EventHandler OnHealthChanged; // BATTLE
    public event EventHandler OnHealthMaxChanged; // PREPARATION
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead; // BATTLE
    public event EventHandler OnAttackChanged; // BATTLE
    public event EventHandler OnAttackMaxChanged; // PREPARATION
    public event EventHandler OnNerfed;
    public event EventHandler OnBuffed;
    //public event EventHandler OnReset;

    private int healthMax; // Max health of animal (Change this during the preparation phase & reset health to this after battle phase)
    private int health; // Current health of animal (Change this during the battle phase)
    private int attackMax; // Max attack of animal (Change this during the preparation phase & reset attack to this after battle phase)
    private int attack; // Current attack of animal (Change this during the battle phase)

    /// <summary>
    /// Construct a StatsTracker, receives the health & attack max and sets current health and attack to that value
    /// </summary>
    public StatsTracker(int healthMax, int attackMax) {
        this.healthMax = healthMax;
        health = healthMax;
        this.attackMax = attackMax;
        attack = attackMax;
    }

    // -------------------------- GETTER FUNCTIONS -------------------------------
    
    /// <summary>
    /// Get the current health
    /// </summary>
    public int GetHealth() {
        return health;
    }

    /// <summary>
    /// Get the current max amount of health
    /// </summary>
    public int GetHealthMax() {
        return healthMax;
    }

    /// <summary>
    /// Get the current attack
    /// </summary>
    public int GetAttack() {
        return attack;
    }

    /// <summary>
    /// Get the current max amount of attack
    /// </summary>
    public int GetAttackMax() {
        return attackMax;
    }

    // -------------------------- HEALTH FUNCTIONS -------------------------------
    
    /// <summary>
    /// Deal damage to this StatsTracker's Health
    /// Called in the Battle Phase
    /// Health has floor of 0
    /// </summary>
    public void Damage(int amount) {
        health -= amount;
        if (health < 0) {
            health = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }

    /// <summary>
    /// Kill this StatsTracker
    /// Called in the Battle Phase
    /// </summary>
    public void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    /*/// <summary>
    /// Test if this StatsTracker is dead
    /// </summary>
    public bool IsDead() {
        return health <= 0;
    }*/

    /// <summary>
    /// Heal this StatsTracker's Health (Allows to go over HealthMax)
    /// Called in the Battle Phase
    /// Health has ceiling of 99
    /// </summary>
    public void Heal(int amount) {
        health += amount;
        if (health > 99) {
            health = 99;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    /*/// <summary>
    /// Reset this StatsTracker to the maximum health amount
    /// Called at the start of Preparation Phase (After Battle Phase ends)
    /// </summary>
    public void ResetHealth() {
        health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnReset?.Invoke(this, EventArgs.Empty);
    }*/

    /// <summary>
    /// Increases the Max Health Amount, and sets the Health Amount to the new Max
    /// Max Health Amount has a ceiling of 99
    /// Called in Preparation Phase
    /// </summary>
    public void IncreaseHealthMax(int amount) {
        healthMax += amount;
        if (healthMax > 99) {
            healthMax = 99;
        }
        health = healthMax;
        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Set the Max Health Amount, optionally also set the Health Amount to the new Max
    /// Possibly used when initializing the animal
    /// </summary>
    public void SetHealthMax(int healthMax, bool fullHealth) {
        this.healthMax = healthMax;
        if (fullHealth) health = healthMax;
        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Set the current Health amount, doesn't set above healthMax or below 0
    /// Possibly used when initializing the animal
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health) {
        if (health > healthMax) {
            health = healthMax;
        }
        if (health < 0) {
            health = 0;
        }
        this.health = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }

    // -------------------------- ATTACK FUNCTIONS -------------------------------

    /// <summary>
    /// Nerf this StatsTracker's Attack
    /// Called in the Battle Phase
    /// Attack has floor of 0
    /// </summary>
    public void Nerf(int amount) {
        attack -= amount;
        if (attack < 0) {
            attack = 0;
        }
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
        OnNerfed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Buff this StatsTracker's Attack (Allows to go over AttackMax)
    /// Called in the Battle Phase
    /// Attack has ceiling of 99
    /// </summary>
    public void Buff(int amount) {
        attack += amount;
        if (attack > 99) {
            attack = 99;
        }
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
        OnBuffed?.Invoke(this, EventArgs.Empty);
    }

    /*/// <summary>
    /// Reset this StatsTracker to the maximum attack amount
    /// Called at the start of Preparation Phase (After Battle Phase ends)
    /// </summary>
    public void ResetAttack() {
        attack = attackMax;
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
        OnReset?.Invoke(this, EventArgs.Empty);
    }*/

    /// <summary>
    /// Increases the Max Attack Amount, and sets the Attack Amount to the new Max
    /// Max Attack Amount has a ceiling of 99
    /// Called in Preparation Phase
    /// </summary>
    public void IncreaseAttackMax(int amount) {
        attackMax += amount;
        if (attackMax > 99) {
            attackMax = 99;
        }
        attack = attackMax;
        OnAttackMaxChanged?.Invoke(this, EventArgs.Empty);
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Set the Max Attack Amount, optionally also set the Attack Amount to the new Max
    /// Possibly used when initializing the animal
    /// </summary>
    public void SetAttackMax(int attackMax, bool fullAttack) {
        this.attackMax = attackMax;
        if (fullAttack) attack = attackMax;
        OnAttackMaxChanged?.Invoke(this, EventArgs.Empty);
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Set the current Attack amount, doesn't set above attackMax or below 0
    /// Possibly used when initializing the animal
    /// </summary>
    /// <param name="attack"></param>
    public void SetAttack(int attack) {
        if (attack > attackMax) {
            attack = attackMax;
        }
        if (attack < 0) {
            attack = 0;
        }
        this.attack = attack;
        OnAttackChanged?.Invoke(this, EventArgs.Empty);
    }

    // -------------------------- MISCELLANEOUS FUNCTIONS -------------------------------

    /// <summary>
    /// Tries to get a StatsTracker from the GameObject
    /// The GameObject can have either the built in StatsTrackerComponent script or any other script that creates
    /// the StatsTracker and implements the IGetStatsTracker interface
    /// </summary>
    /// <param name="getStatsTrackerGameObject">GameObject to get the StatsTracker from</param>
    /// <param name="statsTracker">output StatsTracker reference</param>
    /// <param name="logErrors">Trigger a Debug.LogError or not</param>
    /// <returns></returns>
    public static bool TryGetStatsTracker(GameObject getStatsTrackerGameObject, out StatsTracker statsTracker, bool logErrors = false) {
        statsTracker = null;

        if (getStatsTrackerGameObject != null) {
            if (getStatsTrackerGameObject.TryGetComponent(out IGetStatsTracker getStatsTracker)) {
                statsTracker = getStatsTracker.GetStatsTracker();
                if (statsTracker != null) {
                    return true;
                } else {
                    if (logErrors) {
                        Debug.LogError($"Got StatsTracker from object but StatsTracker is null! Should it have been created? Maybe you have an issue with the order of operations.");
                    }
                    return false;
                }
            } else {
                if (logErrors) {
                    Debug.LogError($"Referenced Game Object '{getStatsTrackerGameObject}' does not have a script that implements IGetStatsTracker!");
                }
                return false;
            }
        } else {
            // No reference assigned
            if (logErrors) {
                Debug.LogError($"You need to assign the field 'getStatsTrackerGameObject'!");
            }
            return false;
        }
    }
}