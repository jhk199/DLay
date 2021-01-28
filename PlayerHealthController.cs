using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour {

    public static PlayerHealthController instance;
    
    public float currentHealth; 
    public float maxHealth;
    public GameObject hitEffect; // Blood splat
    public int[] dmgSound = new int[] { 12, 13 }; // Random dmg sound

    
    
    public float invinceLength = 1f; // Invincible timer
    private float invinceCount;
    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() { // Set health from Character Tracker between levels
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update() {
        invince(); 
    }

    void invince() {
        // Color change on invince
        if(invinceCount > 0) {
            invinceCount -= Time.deltaTime;

            if(invinceCount <= 0) {
                PlayerController.instance.bodySR.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    public void damagePlayer() {
        if (invinceCount <= 0) {
            

            if(PlayerController.instance.currentPowerups.Count != 0) {
                PlayerController.instance.cloudTimer = 0f; // Resets CloudFog rating to 'F'  
            }
            
            currentHealth--; // oW!
            int random = Random.Range(12, 13);
            AudioManager.instance.playSfx(random);
            PlayerController.instance.takeDamage();
            Instantiate(hitEffect, PlayerController.instance.transform.position, PlayerController.instance.transform.rotation); // blood
            invinceCount = invinceLength;

            PlayerController.instance.bodySR.color = new Color(0.77f, 0.33f, 0.33f, PlayerController.instance.bodySR.color.a); // Red color
            
            // DIE!
            if (currentHealth <= 0 && SceneManager.GetActiveScene().name == "Tutorial") {
                currentHealth = 1;
            }
            if (currentHealth <= 0) { 


                AudioManager.instance.playSfx(11);
                
                Time.timeScale = 0f; // Freeze game
                UIController.instance.deathScreen.SetActive(true); // Death screen
                AudioManager.instance.gameOver(); // Play death Music
            }
            LevelManager.instance.pingIncrease(10); // Increase Ping when taking dmg
            // Update UI
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void makeInvincible(float length) { // USed by PlayerController
        invinceCount = length;
    }

    public void healPlayer(float amount) { // Reverse of damage!
        currentHealth += amount;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void increaseMaxHealth(float amount) { // Used by Higher Bandwidth Powerup. Gives more hp too
        maxHealth += amount;
        currentHealth += amount;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void increaseMax(float amount) { // Just increases max hp, doesn't heal
        maxHealth += amount;
        
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }


}
