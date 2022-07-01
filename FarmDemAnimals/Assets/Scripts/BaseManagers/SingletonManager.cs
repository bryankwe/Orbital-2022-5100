using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T: SingletonManager<T> {
    public static T Instance;

    protected void Awake() {

        if (Instance == null) {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
