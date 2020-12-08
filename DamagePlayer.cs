using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
     
    }

    // Update is called once per frame
    void Update()  {
        
    }

    private void OnTriggerEnter2D(Collider2D other) { // Collision for enemy bullet
        if(other.tag == "Player") {
            PlayerHealthController.instance.damagePlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D other) { // Trap collision
        if (other.tag == "Player") {
            PlayerHealthController.instance.damagePlayer();
        }
    }
    private void OnCollisionEnter2D(Collision2D other) { // Trap enter collision
        if (other.gameObject.tag == "Player") {
            PlayerHealthController.instance.damagePlayer();
        }
    }

    private void OnCollisionsStay2D(Collision2D other) { // Unused for now
        if (other.gameObject.tag == "Player") {
            PlayerHealthController.instance.damagePlayer();
        }
    }
}
