using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WarbandData", menuName = "WarbandDataSO/WarbandData")]
public class WarbandDataSO : ScriptableObject {
    
    [System.Serializable]
    public struct EntityData {
        public int animalID;
        public int attack;
        public int health;
        public int position;
        public int totalEntityCount;

        public EntityData(int animalID, int attack, int health, int position, int totalEntityCount) {
            this.animalID = animalID; // For reference to EntitiesDatabaseSO
            this.attack = attack; // To set attack
            this.health = health; // To set health
            this.position = position; // For correct positioning in warband
            this.totalEntityCount = totalEntityCount; // Only for back to Preparation Phase
        }
    }

    /// <summary> 
    /// List of information (EntityData) about animals in warband
    /// For instantiating a prefab and subsequently changing stats
    /// </summary>
    public List<EntityData> warbandEntities;
}
