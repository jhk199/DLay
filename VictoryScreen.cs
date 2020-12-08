using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour {

    public float wait = 2f;
    public GameObject anyKeyText;
    public string mainMenu;
    
    void Start() {
        Time.timeScale = 1f;
        // NO MULTIPLE PLAYERS!!!!
        Destroy(PlayerController.instance.gameObject);
        Destroy(CharacterTracker.instance.gameObject);
    }

    // Update is called once per frame
    void Update() {
        // Go back to main menu after a bit
        if(wait > 0) {
            wait -= Time.deltaTime;
            if(wait <= 0) {
                anyKeyText.SetActive(true);
            }
        }
        else {
            if(Input.anyKeyDown) { // Load main menu
                SceneManager.LoadScene(mainMenu);
            }
        }
    }
}
