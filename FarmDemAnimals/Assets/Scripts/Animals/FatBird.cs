using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBird : BaseEntity {
    public override Ability ability { get { return Ability.DEATH; } }

    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives all animals behind +2/+2 upon death
    /// Note: If calling this ability, means this FatBirdd is already at first position of BattleManager.XXTeam
    /// </summary>
    public override void ActivateAbilityBeforeDeath() {
        
        base.ActivateAbilityBeforeDeath();
        
        List<BaseEntity> currentTeam = battleRef.playerTeam;
        // If animal is enemy team, assign the team reference to enemyTeam
        if (team == BaseEntity.Team.ENEMY) {
            currentTeam = battleRef.enemyTeam;
        }
        for (int i = 1; i < currentTeam.Count; i++) {
            currentTeam[i].IncreaseBattleStats(2,2);
        }
    }
}
