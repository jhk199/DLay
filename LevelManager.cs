using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tracking a lot of variables
public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    public Room CurrentRoom { get; set; } // what room am I in?
    // self explanatory variables
    public int currentPapers;
    public float currentPing;
    public float futurePing;
    
    public float waitToLoad = 4f; // Timer of fade screen between levels
    public string nextLevel;
    public bool isPaused;
    public Transform startPoint; // Where you start

    private void Awake() {
        instance = this;
    }

    void Start() {
        
        PlayerController.instance.transform.position = startPoint.transform.position; // Spawn player
        PlayerController.instance.canMove = true; // Let player move
        futurePing = CharacterTracker.instance.futurePing; // For powerups (Amiri Alg)
        currentPing = CharacterTracker.instance.ping; // Set ping
        currentPapers = CharacterTracker.instance.currentPapers; // Set research papers
        Time.timeScale = 1f; // Time starts
        // Update text and ping color
        UIController.instance.paperText.text = currentPapers.ToString();
        UIController.instance.pingText.text = currentPing.ToString() + "ms";
        updatePingColor();   
    }
    private void Update() { // Pause | Unpause
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseUnpause();
        }

    }

    public IEnumerator levelEnd() { // End level
        Debug.Log("Loading Next level");
        Debug.Log(nextLevel);
        PlayerController.instance.canMove = false;
        // AudioManager.instance.win();
        UIController.instance.startFadeToBlack();
        yield return new WaitForSeconds(waitToLoad); // Start fadeout
        CharacterTracker.instance.currentPapers = currentPapers;
        CharacterTracker.instance.ping = currentPing;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;
        SceneManager.LoadScene(nextLevel);

        UIController.instance.startFadeOutFromBlack();
    }

    public void pauseUnpause() { // Stop time
        if(!isPaused) {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
            PlayerController.instance.canMove = false;
        }
        else { // Start time
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
            PlayerController.instance.canMove = true;
        }
    }

    public void getPapers(int amount) { // Add more research papers!
        currentPapers += amount;
        UIController.instance.paperText.text = currentPapers.ToString();
    }

    public void spendPapers(int amount) { // Spend your hard earned knowledge!
        currentPapers -= amount;
        if(currentPapers < 0) {
            currentPapers = 0;
        }
        UIController.instance.paperText.text = currentPapers.ToString();
    }

    public void pingIncrease(float amount) { // Make ping up
        float newPing = Mathf.Round(amount / futurePing);
        currentPing += newPing;
        setPing();
    }

    public void pingDecrease(float amount) { // Make ping down
        float newPing = Mathf.Round(amount / futurePing);
        currentPing -= newPing;
        if(currentPing < 0 ) {
            currentPing = 0;
        }
        setPing();
    }

    public void setPing() { // Set ping
        currentPing = Mathf.Round(currentPing);
        UIController.instance.pingText.text = currentPing.ToString() + "ms";
        updatePingColor();
    }
    public void updatePingColor() { // Green good, yellow meh, red BAD
        if(currentPing > 0 && currentPing < 50) {
            UIController.instance.pingText.color = Color.green;
            UIController.instance.pingText.fontSize = 70;
        }
        else if (currentPing >= 50 && currentPing < 100) {
            UIController.instance.pingText.color = Color.yellow;
            UIController.instance.pingText.fontSize = 75;
        }
        else if (currentPing >= 100) {
            UIController.instance.pingText.color = Color.red;
            UIController.instance.pingText.fontSize = 80;
        }
    }
}