using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

    public static ShopItem instance;
    public GameObject buyMessage;
    private bool inBuyZone;
    // what kind of item is it?
    public enum itemType { healthRestore, powerup, weapon, pingdecrease };
    public itemType selectedType;
    // For genereating weapons and powerups in shop
    public bool isWeapon, isPowerup;
    public float itemCost; // $$$$$$$$$
    public int healthUpgradeAmount; // Unused right now
    public int selectedGun, selectedPowerup;
    // Gun to give
    private Gun theGun;
    // Powerup to give
    public Powerups thePowerup;
    // Lists
    public Gun[] potentialGuns;
    public Powerups[] potentialPowerups;
    
    // Sprites
    public SpriteRenderer gunSprite;
    public SpriteRenderer powerupSprite;
    // Text
    public Text infoText;

    private void Awake() {
        instance = this;
    }
    
    void Start() {
        generatePowerup();
        generateGunSale();
    }

    
    void Update()  { // Close enough to buy?
        if (inBuyZone) {
            if(Input.GetKeyDown(KeyCode.E)) {
                if(LevelManager.instance.currentPapers >= itemCost) { // Can buy!
                   itemChoice();  
                }
                else { // Can't buy!
                    AudioManager.instance.playSfx(21);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { // Activate "Press E"
        if(other.tag == "Player") {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
      
    }

    private void OnTriggerExit2D(Collider2D other) { // Disabled "Press E to..."
        if (other.tag == "Player") {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }

    void itemChoice() {
        bool bought = false; // Is the item still here?
        switch (selectedType) {
            case itemType.healthRestore: // Should you get more hp?
                if(PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth) {
                    PlayerHealthController.instance.healPlayer(PlayerHealthController.instance.maxHealth);
                    bought = true;
                }
                
                break;
            case itemType.powerup: // Should you get a powerup? 
                // Function is exact same as powerup pickup with a different list for powerups
                bool hasPowerup = false;
                foreach (Powerups powerup in PlayerController.instance.currentPowerups) {
                    if (thePowerup.powerupName == powerup.powerupName) {
                        hasPowerup = true;
                    }
                }

                if (!hasPowerup) {
                    if (UIController.instance.powerupsPoint.position.x >= 250) {
                        UIController.instance.powerupsPoint.position -= new Vector3(250f, 75f, 0f);
                    }
                    Powerups powerupClone = Instantiate(thePowerup);
                    powerupClone.transform.SetParent(UIController.instance.powerups, false);
                    powerupClone.transform.position = UIController.instance.powerupsPoint.position;
                    PlayerController.instance.currentPowerups.Add(powerupClone);
                    UIController.instance.powerupsPoint.position += new Vector3(50f, 0f, 0f);
                }
                bought = true;
                break;
            case itemType.weapon: // you want gun?
                Gun gunClone = Instantiate(theGun); // make gun
                // Just like gunPickup but you buy it!
                gunClone.transform.parent = PlayerController.instance.gunArm; 
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = new Vector3(.67f, .67f, 1f);
                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.switchGun();
                bought = true;
                break;
            case itemType.pingdecrease: // You want lowerping eh!
                if(LevelManager.instance.currentPing > 20) {
                    LevelManager.instance.pingDecrease(20);
                    bought = true;
                }
                break;
        }
        if (bought == true) { // if bought play pretty sound, spend moolah, destroy item
            AudioManager.instance.playSfx(20);
            LevelManager.instance.spendPapers((int)Mathf.Round(itemCost));
            Destroy(gameObject);
            inBuyZone = false;
        }
        else {
            AudioManager.instance.playSfx(21);
        }
        
    }

    

    public void generateGunSale() { // Pick random gun for shop!
        if (isWeapon) {
            selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];
            // GunArray.instance.gunShopList.Remove(potentialGuns[selectedGun]);
            gunSprite.sprite = theGun.gunShopSprite;
            infoText.text = theGun.gunName + "\n - " + theGun.itemCost + " Research Papers - ";
            itemCost = theGun.itemCost;
        }
    }

    public void generatePowerup() { // Pick random powerup for shop!
        if (isPowerup) {
            selectedPowerup = Random.Range(0, potentialPowerups.Length);
            thePowerup = potentialPowerups[selectedPowerup];
            // GunArray.instance.gunShopList.Remove(potentialGuns[selectedGun]);
            powerupSprite.sprite = thePowerup.powerupUI;
            infoText.text = thePowerup.powerupName + "\n - " + thePowerup.cost + " Research Papers - ";
            itemCost = thePowerup.cost;
        }
    }
}
