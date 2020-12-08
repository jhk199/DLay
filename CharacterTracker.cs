using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour {

    // Script to keep track of important PlayerVariables in betweeen levels
    public static CharacterTracker instance;
    public int  currentPapers;
    public float currentHealth, maxHealth, ping, futurePing;
    private void Awake() {
        instance = this;
    }


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
