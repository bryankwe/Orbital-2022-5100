using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject myPrefab;
    void Start()
    {
        Instantiate(myPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
