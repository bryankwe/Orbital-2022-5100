using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : BaseEntity {

    public override string ability { get { return "KILL";} }

    public override void activateAbility() {
        
    }
    /*public override void Damage(int amount) {
        statsTracker.Damage(amount);
    }

    public override void Heal(int amount) {
        statsTracker.Heal(amount);
    }*/
}
