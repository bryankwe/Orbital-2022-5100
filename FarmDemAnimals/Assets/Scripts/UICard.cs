using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note: ONLY the GameObject under the Shop's Slots are UICards
//       The actual Animals Instantiated are NOT UICards
public class UICard : MonoBehaviour {
    //public GameObject icon;
    //public Text animalName;
    //public BaseEntity display;

    //public Transform parentTrans;
    
    private UIShop shopRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef) {
        //icon = myData.icon;
        //animalName.text = myData.name;
        //display = myData.prefab;

        this.myData = myData;
        this.shopRef = shopRef;
        BaseEntity newCard = Instantiate(myData.prefab, this.transform.position, Quaternion.identity);
        newCard.transform.SetParent(this.transform.parent);
        newCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newCard.transform.SetAsFirstSibling();
        newCard.shopRef = this.shopRef;
        Debug.Log("Generated: " + newCard.name);
    }

    /*public void OnClick() {
        shopRef.OnCardClick(myData);
    }*/
}
