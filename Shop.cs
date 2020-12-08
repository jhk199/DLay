using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    // Shop tracking for deploy server powerup
    private bool hasEntered = false;
    
    void Start() {
        
    }

    
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(hasEntered == false) {
            ShopItem.instance.generateGunSale();
        }
        hasEntered = true;
        if (other.tag == "Player" && PlayerController.instance.deployserver == true) {
            LevelManager.instance.currentPing /= 3f;
            LevelManager.instance.setPing();
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player" && PlayerController.instance.deployserver == true) {
            Destroy(gameObject);
        }
                
    }
}
