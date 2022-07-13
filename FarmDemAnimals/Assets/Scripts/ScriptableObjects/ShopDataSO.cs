using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopData", menuName = "ShopDataSO/ShopData")]
public class ShopDataSO : ScriptableObject {
    
    [System.Serializable]
    public struct EntityData {
        public int animalID;
        public int position;

        public EntityData(int animalID, int position) {
            this.animalID = animalID; // For reference to EntitiesDatabaseSO
            this.position = position; // For correct positioning in warband
        }
    }

    /// <summary> 
    /// List of information (EntityData) about animals in shop that were FROZEN
    /// For instantiating a prefab in the next preparation phase
    /// </summary>
    public List<EntityData> frozenShopEntities;
}
