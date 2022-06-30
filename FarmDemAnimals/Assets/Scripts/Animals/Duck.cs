using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : BaseEntity {
    public override Ability ability { get { return Ability.ENRAGE; } }

    public override void activateAbility() {
        
    }
}
