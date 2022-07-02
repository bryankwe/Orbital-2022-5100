using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager> {

    public void onGoBackClick() {
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }

}
