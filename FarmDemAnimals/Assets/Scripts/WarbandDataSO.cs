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

        public EntityData(int animalID, Sprite animalSprite, int attack, int health, int position) {
            this.animalID = animalID;
            this.animalSprite = animalSprite;
            this.attack = attack;
            this.health = health;
            this.position = position;
        }
    }

    public List<EntityData> warbandEntities;
}
