using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : BaseEntity {
    public override void Damage() {
        statsTracker.Damage(60);
    }
}
