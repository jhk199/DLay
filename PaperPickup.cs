using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPickup : MonoBehaviour {

    public static PaperPickup instance;
    public Animator anim;
    public int paperValue = 1;
    public float waitToBeCollected;
    private void Awake() {
        instance = this;
    }
    void Start() {

    }
    void Update() {
        if(waitToBeCollected > 0) {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { // pickup papers, get papers
        if (other.tag == "Player" && waitToBeCollected <= 0) {
            GetComponent<Collider2D>().enabled = false;
            AudioManager.instance.playSfx(6);
            LevelManager.instance.getPapers(1);
            anim.SetTrigger("pickedUp");
        }
    }
}
