using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "EnemyDatabaseSO/EnemyData")]
public class EnemyDatabaseSO : ScriptableObject {
    
    [System.Serializable]
    public struct TeamData {
        public List<WarbandDataSO.EntityData> warbandTeam;
        public int turnNumber;

        public TeamData(List<WarbandDataSO.EntityData> warbandTeam, int turnNumber) {
            this.warbandTeam = warbandTeam; // List<EntityData>
            this.turnNumber = turnNumber; // Filter by Turn Number
        }
    }

    public List<TeamData> pastTeams;
    
    // Another Way --> Using Dictionary
    // Key: Turn Number (Search using this)
    // Value: List of warband teams (randomise and output one warband team) 
    //public Dictionary<int, List<List<WarbandDataSO.EntityData>>> teamDatabase;
}
