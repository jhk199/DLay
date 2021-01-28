using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Simple script to load menu and start the game!
    public string levelToLoad;
    public string tutorial;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void startGame() {
        SceneManager.LoadScene(levelToLoad);
    }
    public void startTutorial() {
        SceneManager.LoadScene(tutorial);
    }

    public void exitGame() {
        Application.Quit();
    }
}
