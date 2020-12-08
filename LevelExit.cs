using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    // Start level change
    void Start() {
        
    }

    
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            // warp();
            StartCoroutine(LevelManager.instance.levelEnd());
        }
    }
}
