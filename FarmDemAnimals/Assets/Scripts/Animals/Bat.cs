using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : BaseEntity {

    public override Ability ability { get { return Ability.BUY; } } //public override string ability { get { return "BUY";} }
    
    public override void activateAbility() {
        
        List<BaseEntity> currentWarband = PreparationManager.Instance.warband;
        
        if (PreparationManager.Instance.CountWarbandAnimals() > 1) { // If there are other animals in warband
            
            bool executing = true;

            while (executing) {
                
                BaseEntity randomAnimal = currentWarband[Random.Range(0, currentWarband.Count)];
                
                if (randomAnimal != this && randomAnimal != null) {
                    randomAnimal.IncreasePreparationStats(1,1);
                    PreparationManager.Instance.OnUpdateWarband?.Invoke();
                    executing = false;
                    return;
                }
            }
        }
    }
}
