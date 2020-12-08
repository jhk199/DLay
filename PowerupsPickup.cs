using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsPickup : MonoBehaviour {

    public GameObject pickup;
    public GameObject powerupName;
    public static PowerupsPickup instance;
    public Powerups thePowerup;
    public SpriteRenderer icon;
    public float waitToBeCollected = 0.5f;
    private bool inBuyZone;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        icon.sprite = thePowerup.powerupUI;
        powerupName.GetComponentInChildren<Text>().text = thePowerup.powerupName; // Sets text to Powerup name
    }

    // Update is called once per frame
    void Update() {
        if (waitToBeCollected > 0) {
            waitToBeCollected -= Time.deltaTime;
        }
        if (inBuyZone) {
            if (Input.GetKeyDown(KeyCode.E)) {
                    pickupPowerup();     
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other) { // If player tries to pick up powerup
        if (other.tag == "Player") {
            //pickup.SetActive(true); 
            powerupName.SetActive(true);
            pickup.GetComponentInChildren<Text>().color = Color.yellow;
            pickup.GetComponentInChildren<Text>().fontSize = 40;
            inBuyZone = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other) { // Disable tooltip when player leaves zone
        if (other.tag == "Player") {
            //pickup.SetActive(false);
            powerupName.SetActive(false);
            pickup.GetComponentInChildren<Text>().color = Color.white;
            pickup.GetComponentInChildren<Text>().fontSize = 30;
            inBuyZone = false;
        }
    }

    void pickupPowerup() { // Assigning the powerup
        bool hasPowerup = false;
        foreach (Powerups powerup in PlayerController.instance.currentPowerups) {
            if (thePowerup.powerupName == powerup.powerupName) {
                hasPowerup = true; // Checks if player already has powerup
            }
        }
               
        if (!hasPowerup) {
            if(UIController.instance.powerupsPoint.position.x >= 250 ) { // UI canvas shift if player has more than 5 powerups
                UIController.instance.powerupsPoint.position -= new Vector3(250f, 75f, 0f);
            }
            Powerups powerupClone = Instantiate(thePowerup);
            powerupClone.transform.SetParent(UIController.instance.powerups, false);
            powerupClone.transform.position = UIController.instance.powerupsPoint.position;
            PlayerController.instance.currentPowerups.Add(powerupClone);
            UIController.instance.powerupsPoint.position += new Vector3(50f, 0f, 0f);
        }
        Destroy(gameObject);
           
        
    }
}



