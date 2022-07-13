using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : BaseEntity {
    public override Ability ability { get { return Ability.DEATH; } }

    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives the 2 animals behind +1/+1 upon death
    /// Note: If calling this ability, means this BlueBird is already at first position of BattleManager.XXTeam
    /// </summary>
    public override void ActivateAbilityBeforeDeath() {
        
        base.ActivateAbilityBeforeDeath();
        
        List<BaseEntity> currentTeam = battleRef.playerTeam;
        // If animal is enemy team, assign the team reference to enemyTeam
        if (team == BaseEntity.Team.ENEMY) {
            currentTeam = battleRef.enemyTeam;
        }

        if (currentTeam.Count == 2) { // If there is only one other animal other than this Blue Bird
            BaseEntity animalBehind = currentTeam[1];
            animalBehind.IncreaseBattleStats(1,1);
        } else if (currentTeam.Count > 2) { // If there is at least 2 other animals other than this Blue Bird
            BaseEntity firstAnimalBehind = currentTeam[1];
            BaseEntity secondAnimalBehind = currentTeam[2];
            firstAnimalBehind.IncreaseBattleStats(1,1);
            secondAnimalBehind.IncreaseBattleStats(1,1);
        }
    }
}
