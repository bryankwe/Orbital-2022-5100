using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBird : BaseEntity {
    public override Ability ability { get { return Ability.DEATH; } }

    public override void activateAbility() {
        
    }

    /// <summary>
    /// Gives all animals behind +2/+2 upon death
    /// Note: If calling this ability, means this FatBirdd is already at first position of BattleManager.playerTeam
    /// </summary>
    public override void ActivateAbilityBeforeDeath() {
        List<BaseEntity> currentTeam = battleRef.playerTeam;
        for (int i = 1; i < currentTeam.Count; i++) {
            currentTeam[i].IncreaseBattleStats(2,2);
        }
    }
}
