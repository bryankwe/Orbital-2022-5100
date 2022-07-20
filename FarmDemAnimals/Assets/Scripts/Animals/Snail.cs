using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : BaseEntity {
    public override Ability ability { get { return Ability.TURNEND; } }

    public override void activateAbility() {
        
        List<BaseEntity> currentWarband = PreparationManager.Instance.warband;
        
        if (PreparationManager.Instance.CountWarbandAnimals() > 1) { // If there are other animals in warband
            
            int selfIndex = currentWarband.FindIndex(x => x == this); // Find self index

            for (int i = selfIndex - 1; i >= 0; i--) { // Loop through from self position to first
                if (currentWarband[i] != null) { // skip if empty
                    currentWarband[i].IncreasePreparationStats(1,1);
                    return;
                }
            }
        }
    }
}