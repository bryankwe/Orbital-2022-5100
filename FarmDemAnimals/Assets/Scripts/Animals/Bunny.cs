using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : BaseEntity {
    public override Ability ability { get { return Ability.TURNSTART; } }

    public override void activateAbility() {
        Debug.Log("Bunny Ability Activated");
        PlayerData.Instance.AddMoney(2);
        //this.shopRef.Refresh();
    }
}
