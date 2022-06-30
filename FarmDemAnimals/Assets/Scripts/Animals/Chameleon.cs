using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : BaseEntity {
    public override Ability ability { get { return Ability.TURNEND; } }

    public override void activateAbility() {
        
        List<BaseEntity> currentWarband = PreparationManager.Instance.warband;
        
        if (PreparationManager.Instance.CountWarbandAnimals() > 1) { // If there are other animals in warband
            
            for (int i = 0; i < currentWarband.Count; i++) {
                BaseEntity currentAnimal = currentWarband[i];
                if (currentAnimal == this) { // if no animals in front of self
                    return;
                } else if (currentAnimal != null) {
                    currentAnimal.IncreasePreparationStats(2, 3);
                    return;
                }
            }
        }
    }
}
