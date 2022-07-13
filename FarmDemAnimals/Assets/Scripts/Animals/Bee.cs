using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : BaseEntity {

    public override Ability ability { get { return Ability.DEATH; } } //public override string ability { get { return "DEATH";} }

    // Meant to be empty -> NO Preparation Phase ability
    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives the animal behind +1/+1 upon death
    /// Note: If calling this ability, means this Bee is already at first position of BattleManager.playerTeam
    /// </summary>
    public override void ActivateAbilityBeforeDeath() {
        List<BaseEntity> currentTeam = battleRef.playerTeam;
        if (currentTeam.Count > 1) { // If there are other animals other than this Bee
            BaseEntity animalBehind = currentTeam[1];
            animalBehind.IncreaseBattleStats(1,1);
            Debug.Log("Bee increased stats for " + animalBehind.name);
        }
    }
}
