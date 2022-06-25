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

    /// <summary>
    /// Deactivate the shop slot on awake
    /// </summary>
    private void Awake() {
        transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activate the shop slot on awake
    /// </summary>
    public void EnableCard() {
        transform.parent.gameObject.SetActive(true);
    }

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

    /// <summary>
    /// Returns whether or not the UICard is active in scene
    /// Determine whether or not to instantiate an animal on this UICard
    /// </summary>
    public bool CanGenerate() {
        return gameObject.activeInHierarchy;
    }
}
