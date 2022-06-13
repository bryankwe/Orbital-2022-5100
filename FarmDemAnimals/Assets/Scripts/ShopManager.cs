using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    
    public EntitiesDatabaseSO entitiesDatabase;

    /*
    NOTE: JUST COPYING CODE FROM TUTORIAL (NEED TO THINK HOW TO ADAPT TO OUR GAME)
    //OLD VERSION (FOR non-UI draggable)
    //Reference Tutorial: TaroDev (https://www.youtube.com/watch?v=Tv82HIvKcZQ) & (https://www.youtube.com/watch?v=o_qEXZhQR-M)

    [SerializeField] private List<DropSlot> slots;
    [SerializeField] private Draggable draggable;
    [SerializeField] private Transform slotParent, draggableParent;
    
    void Spawn() {
        var randomSet = slots.OrderBy(s => Random.value).Take(3).ToList();

        for (int i = 0; i < randomSet.Count; i++) {
            var spawnedSlot = Instantiate(randomSet[i], slotParent.GetChild(i).position, Quaternion.identity);

            var spawnedDraggable = Instantiate(draggable, draggableParent.GetChild(i).position, Quaternion.identity);
        }
    }*/
}
