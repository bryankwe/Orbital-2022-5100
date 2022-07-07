using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WarbandData", menuName = "WarbandDataSO/WarbandData")]
public class WarbandDataSO : ScriptableObject {
    
    [System.Serializable]
    public struct EntityData {
        public int animalID;
        public Sprite animalSprite;
        public int attack;
        public int health;
        public int position;
        public int totalEntityCount;
        public UIShop shopRef;

        public EntityData(int animalID, Sprite animalSprite, int attack, int health, int position, int totalEntityCount, UIShop shopRef) {
            this.animalID = animalID; // For reference to EntitiesDatabaseSO
            this.animalSprite = animalSprite; // May Not Need
            this.attack = attack; // To set attack
            this.health = health; // To set health
            this.position = position; // For correct positioning
            this.totalEntityCount = totalEntityCount; // Only for back to Preparation Phase
            this.shopRef = shopRef; // Only for back to Preparation Phase
        }
    }

    public List<EntityData> warbandEntities;
}
