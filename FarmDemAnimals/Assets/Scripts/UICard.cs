using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour {
    //public GameObject icon;
    //public Text animalName;
    //public BaseEntity display;

    public Transform parentTrans;
    
    private UIShop shopRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef) {
        //icon = myData.icon;
        //animalName.text = myData.name;
        //display = myData.prefab;

        this.myData = myData;
        this.shopRef = shopRef;
        BaseEntity newCard = Instantiate(myData.prefab, this.transform.position, Quaternion.identity);
        newCard.transform.SetParent(parentTrans);
        Debug.Log("Generated: " + newCard.name);
    }

    /*public void OnClick() {
        shopRef.OnCardClick(myData);
    }*/
}
