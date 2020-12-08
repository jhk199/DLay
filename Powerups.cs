using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour {

    public static Powerups instance;
    public Sprite powerupUI; // Sprite
    public float cost;
    public string powerupName;
    public int hpChange;
    public int pingChange;
    public int futurePingChange = 1;
    public int firespeedChange;
    public int enemyBulletSpeedChange;
    public int rollChange;
    public int shopCostChange;
    public int finalBossHPChange;
    public int speedChange;
    [HideInInspector]
    public int enemiesInRoom;
    public enum powerup { amirialg, caileung, cloudfog, cloudgameloop, cloudlet, deployserver, dynamiccodecs, 
                          edgegame, edgenode, edgeserver, fiberoptic, gcloud, higherband, hpc, icloud }; // Selection of all powerups
    public powerup selectedPowerup; // For switch
    



    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
        theCases(); // Run switch case
        
    }

    // Update is called once per frame
    void Update() {
        // Bools are all stored in PlayerController as there is only one instance and doesn't screw up other powerups
        if(PlayerController.instance.cloudgameloop) {
            cloudGameLoop();
        }
        if(PlayerController.instance.cloudfog) {
            cloudFogLoop();
        }
        if (PlayerController.instance.hpc) {
            hpcLoop();
        }
    }

    public void theCases() {
        switch(selectedPowerup) {
            case powerup.amirialg:
                // Cut ping by 1/3. All future ping increases decreased by 1/3 as well
                LevelManager.instance.currentPing /= 3f;
                LevelManager.instance.setPing();
                if (LevelManager.instance.futurePing == 1) {
                    LevelManager.instance.futurePing = 3f;
                    CharacterTracker.instance.futurePing = 3f;
                }
                PlayerController.instance.dashSpeed -= 3;
                break;
            case powerup.caileung:
                // Final Boss Hp reduced to 67%
                break;
            case powerup.cloudfog:
                // Powerup: Decrease ping by 20ms. Player gains a reputation score 
                // (starts at F, goes to S). The longer the player doesn’t get hit, 
                // the faster they move. Getting hit resets reputation score to F
                PlayerController.instance.rating.SetActive(true);
                PlayerController.instance.cloudfog = true;
                LevelManager.instance.pingDecrease(20);
                
                break;
            case powerup.cloudgameloop:
                // decreases roll cooldown based on enemies in room
                PlayerController.instance.cloudgameloop = true;
                break;
            case powerup.cloudlet:
                // Shows next room when clearing it
                PlayerController.instance.oneHop = true;
                break;
            case powerup.deployserver:
                // Cuts ping by 1/3 when entering shop
                PlayerController.instance.deployserver = true;
                break;
            case powerup.dynamiccodecs:
                // Slows bullets
                PlayerController.instance.dynamiccodex = true;
                break;
            case powerup.edgegame:
                // Making sure end room is always revealed
                LevelGenerator.instance.generatedOutlines[LevelGenerator.instance.generatedOutlines.Count - 1].GetComponentInChildren<Room>().outline.SetActive(true);
                LevelGenerator.instance.generatedOutlines[LevelGenerator.instance.generatedOutlines.Count - 1].GetComponentInChildren<Room>().mapHider.SetActive(false);
                LevelManager.instance.pingIncrease(20);
                PlayerController.instance.edgegame = true;
                break;
            case powerup.edgenode:
                break;
            case powerup.edgeserver:
                break;
            case powerup.fiberoptic:
                // Speed up
                PlayerController.instance.moveSpeedPowerup *= 1.2f;
                break;
            case powerup.gcloud:
                break;
            case powerup.higherband:
                // More hp
                float doubleMax = Mathf.Round(PlayerHealthController.instance.maxHealth);
                PlayerHealthController.instance.increaseMaxHealth(doubleMax);
                break;
            case powerup.hpc:
                PlayerController.instance.hpc = true;
                break;
            case powerup.icloud:
                break;


        }
    }

    void cloudGameLoop() { // Decrease roll cd when more enemies are present
        if(enemiesInRoom > 0) {
            PlayerController.instance.dashCooldown = (1f/(enemiesInRoom));
        }
        if(enemiesInRoom == 0) {
            PlayerController.instance.dashCooldown = 1;
        }  
    }

    void hpcLoop() { // Shoot quicker
        if (enemiesInRoom > 0) {
            Gun.instance.timeShotPowerup = (1f / (enemiesInRoom));
        }
        if (enemiesInRoom == 0) {
            Gun.instance.timeShotPowerup = 1;
        }
    }

    void cloudFogLoop() { // Rating! Totally not inspired by another game... Speed up more time player isn't shot
        float timer = PlayerController.instance.cloudTimer;
        float msp = PlayerController.instance.moveSpeedPowerup; // Make sure other speedups work
        if (timer < 2) {
            PlayerController.instance.ratingText.text = "F";
            PlayerController.instance.ratingText.color = Color.red;
            PlayerController.instance.activeMoveSpeed = 4f * msp;
            PlayerController.instance.moveSpeed = 4f * msp;
        }
        if(timer > 2 && timer < 4) {
            PlayerController.instance.ratingText.text = "E";
            PlayerController.instance.activeMoveSpeed = 4.5f * msp;
            PlayerController.instance.moveSpeed = 4.5f * msp;
        }
        if (timer > 4 && timer < 6) {
            PlayerController.instance.ratingText.text = "D";
            PlayerController.instance.ratingText.color = Color.yellow;
            PlayerController.instance.activeMoveSpeed = 5f * msp;
            PlayerController.instance.moveSpeed = 5f * msp;
        }
        if (timer > 6 && timer < 8) {
            PlayerController.instance.ratingText.text = "C";
            PlayerController.instance.activeMoveSpeed = 5.5f * msp;
            PlayerController.instance.moveSpeed = 5.5f * msp;
        }
        if (timer > 8 && timer < 10) {
            PlayerController.instance.ratingText.text = "B";
            PlayerController.instance.ratingText.color = Color.cyan;
            PlayerController.instance.activeMoveSpeed = 6.0f * msp;
            PlayerController.instance.moveSpeed = 6.0f * msp;
        }
        if (timer > 10 && timer < 15 ) {
            PlayerController.instance.ratingText.text = "A";
            PlayerController.instance.activeMoveSpeed = 6.5f * msp;
            PlayerController.instance.moveSpeed = 6.5f * msp;
        }
        if (timer > 15) {
            PlayerController.instance.ratingText.text = "S";
            PlayerController.instance.ratingText.color = Color.green;
            PlayerController.instance.activeMoveSpeed = 8.0f * msp;
            PlayerController.instance.moveSpeed = 8.0f * msp;
        }
    }
}
