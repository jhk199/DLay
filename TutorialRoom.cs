using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoom : MonoBehaviour {
    // Start is called before the first frame update
    public static TutorialRoom instance;
    public GameObject text;
    public bool hasEntered = false;
    public enum tutorialEnable {none, health, papers, ping, map}
    public tutorialEnable selectedTutorial;
    

    private void Awake() {
        instance = this;
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if((CameraController.instance.prevPos.x == transform.position.x) && (CameraController.instance.prevPos.y == transform.position.y)) {
            hasEntered = true;
        }
        if(hasEntered == true) {
            text.SetActive(true);
            switchCase();
        }
        
    }

    void switchCase() {
        switch(selectedTutorial) {
            case tutorialEnable.none:
                break;
            case tutorialEnable.health:
                Tutorial.instance.health = true;
                break;
            case tutorialEnable.papers:
                Tutorial.instance.papers = true;
                break;
            case tutorialEnable.ping:
                Tutorial.instance.ping = true;
                break;
            case tutorialEnable.map:
                Tutorial.instance.map = true;
                break;
        }
    }

    

    private void OnTriggerExit2D(Collider2D other) {       
        if(other.tag == "Player") {
            Destroy(gameObject);
        }
           
    }
}
