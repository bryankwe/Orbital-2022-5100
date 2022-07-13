using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : BaseEntity {

    public override Ability ability { get { return Ability.KILL; } } //public override string ability { get { return "KILL";} }

    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives itself +3/+1 if it kills its target and survives
    /// Note: If calling this ability, means this Rhino has already killed its enemy
    /// </summary>
    public override void ActivateKillAbility() {
        // First, check if this Rhino is dead
        if (IsDead()) {
            // Do nothing
            return;
        }
        // This rhino is not dead => Increase its battle stats
        IncreaseBattleStats(3,1);
        Debug.Log("Increased stats for itself");
    }
}
