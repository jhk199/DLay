using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public static HealthPickup instance;
    public Animator anim;
    public int healAmount = 1;
    // public float size = 1.4f;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) { // Heal player when picking up data mail. Only works if player can heal
        if(other.tag == "Player" && PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth) {
            GetComponent<Collider2D>().enabled = false;
            AudioManager.instance.playSfx(8);
            PlayerHealthController.instance.healPlayer(healAmount);
            LevelManager.instance.pingDecrease(5);
            anim.SetTrigger("pickedUp");
            // other.transform.localScale *= size;
        }
    }

}
