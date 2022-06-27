using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : BaseEntity {

    public override string ability { get { return "BUY";} }
    
    private void OnEnable() {
        //PreparationManager.Instance.OnBuy += Bought;
    }

    private void OnDisable() {
        //PreparationManager.Instance.OnBuy -= Bought;
    }
    
    // Why do all Bats call this event? ==> Because of OnEnable() ==> Need to only Bought() when actually buying and not OnEnable()
    // Incomplete ==> Did not cater for Combine()
    public override void activateAbility() { // private void Bought() {
        //Debug.Log("Bought Event Called by " + gameObject.name);
        List<BaseEntity> currentWarband = PreparationManager.Instance.warband;
        //Debug.Log("Warband Animals Count: " + PreparationManager.Instance.CountWarbandAnimals());
        //Debug.Log(" Same Check: " + (currentWarband[0] == currentWarband[1]));
        if (PreparationManager.Instance.CountWarbandAnimals() > 1) { // If there are other animals in warband
            bool executing = true;
            while (executing) {
                BaseEntity randomAnimal = currentWarband[Random.Range(0, currentWarband.Count)];
                //Debug.Log("Selected animal: " + randomAnimal.name);
                if (randomAnimal != this && randomAnimal != null) {
                    randomAnimal.IncreasePreparationStats(1,1);
                    PreparationManager.Instance.OnUpdateWarband?.Invoke();
                    executing = false;
                    return;
                }
            }
        }
    }

    /*public override void Damage(int amount) {
        statsTracker.Damage(amount);
    }

    public override void Heal(int amount) {
        statsTracker.Heal(amount);
    }*/
}
