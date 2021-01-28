using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
    public static Tutorial instance;
    public bool isTutorial = false;
    public GameObject[] objects;
    /*
     * 0: Hand
     * 1: HP
     * 2: Paper
     * 3: Ping
     * 4: Map
     * 5: Current Gun
     * 6: EdgeServer Charges
     */
    public bool health = false, papers = false, ping = false, map = false;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start() {
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            isTutorial = true;
            for (int i = 1; i < objects.Length; i++) {
                objects[i].SetActive(false);
            }
            
        }
    }

    // Update is called once per frame
    void Update() {
        if(isTutorial == true) {
            enableUI();
        }
    }

    void enableUI() {
        if(health == true) {
            objects[1].SetActive(true);
        }
        if(papers == true) {
            objects[2].SetActive(true);
        }
        if (ping == true) {
            objects[3].SetActive(true);
        }
        if (map == true) {
            objects[4].SetActive(true);
        }
        if (PlayerController.instance.availableGuns.Count != 0) {
            objects[0].SetActive(false);
            objects[5].SetActive(true);
        }
    }
}
