using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour {
    public GameObject icon;
    public Text animalName;

    private UIShop shopRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef) {
        icon = myData.icon;
        animalName.text = myData.name;

        this.myData = myData;
        this.shopRef = shopRef;
    }

    /*public void OnClick() {
        shopRef.OnCardClick(myData);
    }*/
}
