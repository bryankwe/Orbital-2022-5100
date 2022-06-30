using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : BaseEntity {
    public override Ability ability { get { return Ability.BUY; } }

    public override void activateAbility() {
        
        List<BaseEntity> currentWarband = PreparationManager.Instance.warband;
        
        if (PreparationManager.Instance.CountWarbandAnimals() == 2) { // If there is only one other animal in warband
            
            bool executing = true;

            while (executing) {
                
                BaseEntity randomAnimal = currentWarband[Random.Range(0, currentWarband.Count)];
                
                if (randomAnimal != this && randomAnimal != null) {
                    randomAnimal.IncreasePreparationStats(2,1);
                    PreparationManager.Instance.OnUpdateWarband?.Invoke();
                    executing = false;
                    return;
                }
            }
        } else if (PreparationManager.Instance.CountWarbandAnimals() > 2) { // If there are at least 2 other animals in warband
            
            bool executing = true;

            while (executing) {
                
                BaseEntity randomAnimalOne = currentWarband[Random.Range(0, currentWarband.Count)];
                BaseEntity randomAnimalTwo = currentWarband[Random.Range(0, currentWarband.Count)];
                
                if (randomAnimalOne != this && randomAnimalOne != null && randomAnimalTwo != this && randomAnimalTwo != null && randomAnimalOne != randomAnimalTwo) {
                    randomAnimalOne.IncreasePreparationStats(2,1);
                    randomAnimalTwo.IncreasePreparationStats(2,1);
                    PreparationManager.Instance.OnUpdateWarband?.Invoke();
                    executing = false;
                    return;
                }
            }
        }
    }
}