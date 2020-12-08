using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour {

    public GunPickup[] potentialGuns; // List of possible guns in chest
    public SpriteRenderer theSR; // Sprite
    public Sprite chestOpen;
    public GameObject notification;
    private bool canOpen, isOpen;
    public Transform spawnPoint;
    public float scaleSpeed = 2; // For grow/shrink effect
    
    
    
    void Start() {
        
    }

    
    void Update() {
        
        openChest();
    }

    private void OnTriggerEnter2D(Collider2D other) { // If there are guns to be picked up and collision is player
        if(other.tag == "Player" && !isOpen && GunArray.instance.empty == false) {
            notification.SetActive(true); // "Press E to....
            canOpen = true; // Bool to open chest
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" ) {
            // IF player leaves collision zone
            notification.SetActive(false);
            canOpen = false;
        }
    }

    private void openChest() {
        // If canOpen is true, the chest isn't open and there are guns in the GunArray
        if(canOpen && !isOpen && GunArray.instance.empty == false) {
            if(Input.GetKeyDown(KeyCode.E)) {
                int gunSelect;
                bool loop = true;
                // Loop guarantees that player gets gun they don't have
                while(loop) {
                    gunSelect = Random.Range(0, potentialGuns.Length);
                    if(GunArray.instance.gunList.Contains(potentialGuns[gunSelect])) {
                        Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
                        GunArray.instance.gunList.Remove(potentialGuns[gunSelect]);
                        loop = false;
                    }
                    
                }
                // Chest animation
                theSR.sprite = chestOpen;
                AudioManager.instance.playSfx(29);
                isOpen = true;
                notification.SetActive(false);
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            }
        }
        if(isOpen) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }
}
