using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : BaseEntity {
    public override Ability ability { get { return Ability.ENRAGE; } }

    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives itself +2/+1 if it takes damage and survives
    /// Note: If calling this ability, means this Duck has already taken damage from its enemy
    /// </summary>
    public override void ActivateAbilityAfterDamaged() {
        // Check if this duck is still alive
        if (IsDead()) {
            // Do nothing
            return;
        }
        // This duck is not dead => Increase its battle stats
        IncreaseBattleStats(2,1);
        Debug.Log("Duck Increased stats for itself");
    }
}
