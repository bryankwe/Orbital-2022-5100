using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject myPrefab;

    public GameManager gameManager;
    void Start()
    {
        /*
        for(int i=0; i < ) {       
            Instantiate(prefab, )
        }
        */
        Instantiate(myPrefab, new Vector3(2,2,2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
