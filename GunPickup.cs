using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour {

    public Gun theGun;
    public float waitToBeCollected = 0.5f; // wait timer
    
    
    void Start() {
        
    }

    
    void Update() { // Timer for waitToBeCollect
        if (waitToBeCollected > 0) {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { // If the player collides with a gun pickup
        if (other.tag == "Player" && waitToBeCollected <= 0) {
            bool hasGun = false; // does the player have the gun?
            foreach(Gun gun in PlayerController.instance.availableGuns) { // iterate through availableGuns List
                if(theGun.gunName == gun.gunName) {
                    hasGun = true;
                }
            }
            if(!hasGun) { // If the player doesn't have the gun
                Gun gunClone = Instantiate(theGun); // Create the gun
                // Next 4 lines assign the gun to the player and give it correct size
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = new Vector3(.67f, .67f, 1f);
                PlayerController.instance.availableGuns.Add(gunClone);
                // Sets gun to active
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.switchGun(); // Updates UI
            }
            Destroy(gameObject); // Removes pickup
            AudioManager.instance.playSfx(7);
        }
    }
}