using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupChest : MonoBehaviour {
    // Just like gunChest but gives player a powerup to pickup SEE GUNCHEST FOR MORE DETAILS
    public PowerupsPickup[] potentialPowerups;
    public SpriteRenderer theSR;
    public Sprite chestOpen;
    public GameObject notification;
    private bool canOpen, isOpen;
    public Transform spawnPoint;
    public float scaleSpeed = 2;



    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        openChest();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !isOpen && GunArray.instance.empty == false) {
            notification.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            notification.SetActive(false);
            canOpen = false;
            //AudioManager.instance.playSfx(28);
        }
    }

    private void openChest() { // Just like gunChest but gives player a powerup to pickup
        if (canOpen && !isOpen && GunArray.instance.empty == false) {
            if (Input.GetKeyDown(KeyCode.E)) {
                int powerupSelect;
                bool loop = true;
                while (loop) {

                    powerupSelect = Random.Range(0, potentialPowerups.Length);
                    if (PowerupArray.instance.powerupList.Contains(potentialPowerups[powerupSelect])) {
                        Instantiate(potentialPowerups[powerupSelect], spawnPoint.position, spawnPoint.rotation);
                        PowerupArray.instance.powerupList.Remove(potentialPowerups[powerupSelect]);
                        loop = false;
                    }

                }

                theSR.sprite = chestOpen;
                AudioManager.instance.playSfx(29);
                isOpen = true;
                notification.SetActive(false);
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            }
        }
        if (isOpen) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }
}
