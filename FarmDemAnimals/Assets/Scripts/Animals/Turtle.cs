using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : BaseEntity {
    public override Ability ability { get { return Ability.COMBINE; } }

    public override void activateAbility() {
        IncreasePreparationStats(2, 1);
        PreparationManager.Instance.OnUpdateWarband?.Invoke();
    }
}