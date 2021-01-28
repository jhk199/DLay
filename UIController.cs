using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Make game pretty, player need see information
public class UIController : MonoBehaviour {

    public static UIController instance;
   // Hp
    public Slider healthSlider;
    // Text
    public Text healthText, paperText, pingText;
   // UI screens for death and fading
    public GameObject deathScreen;
    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadeToBlack, fadeOutBlack;
    // For Death Menu buttons
    public string newGameScene, mainMenuScene;
    // For pause menu and maps
    public GameObject pauseMenu, bigMapText, minimapText;
    // Tracking where powerup icons go
    public Transform powerups;
    public Transform powerupsPoint;
    public GameObject serverCharges;
    public GameObject[] servers;
    // Gun info
    public Image currentGun;
    public Text gunText;

    public Slider bossHealthBar;

    private void Awake() {
        instance = this;
        
    }
    
    void Start() {
        // Fade in, set current gun UI
        fadeOutBlack = true;
        fadeToBlack = false;
        if (SceneManager.GetActiveScene().name != "Tutorial") {
            currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;
            gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunName;
        }
        
    }

   
    void Update() { // Fade in or out
        if(fadeOutBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f) {
                fadeOutBlack = false;
            }
        }
        if(fadeToBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f) {
                fadeToBlack = false;
            }
        }
        if(SceneManager.GetActiveScene().name == "Boss Level") {
            minimapText.SetActive(false);
        }
        
    }

    // Next functions are used by LevelManager
    public void startFadeToBlack() {
        fadeToBlack = true;
        fadeOutBlack = false;
    }
    public void startFadeOutFromBlack() {
        fadeToBlack = false;
        fadeOutBlack = true;
    }

    // No duplicate players!!!
    public void newGame() {
        SceneManager.LoadScene(newGameScene);
        Destroy(PlayerController.instance.gameObject);
        Destroy(CharacterTracker.instance.gameObject);
    }

    // No duplicate players!!!
    public void mainMenu() {
        Destroy(this);
        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
        Destroy(CharacterTracker.instance.gameObject);
    }

    // for pause menu
    public void resume() {
        LevelManager.instance.pauseUnpause();
    }

    
}
