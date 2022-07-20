using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note: ONLY the GameObject under the Shop's Slots are UICards
//       The actual Animals Instantiated are NOT UICards
public class UICard : MonoBehaviour {
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

    /// <summary>
    /// Correctly instantiates the animal based on struct from EntitiesDatabaseSO.EntityData
    /// Shows the frozen icon if animal was previously frozen from last turn
    /// </summary>
    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef, bool isFrozen) {
        this.myData = myData;
        this.shopRef = shopRef;
        
        BaseEntity newCard = Instantiate(myData.prefab, this.transform.position, Quaternion.identity);
        newCard.transform.SetParent(this.transform.parent);
        newCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newCard.transform.SetAsFirstSibling();
        newCard.shopRef = this.shopRef;
        if (isFrozen) {
            newCard.FreezeToggle();
        }
        
        //Debug.Log("Generated: " + newCard.name);
    }

    /// <summary>
    /// Returns whether or not the UICard is active in scene
    /// Determine whether or not to instantiate an animal on this UICard
    /// </summary>
    public bool CanGenerate() {
        return gameObject.activeInHierarchy;
    }
}
