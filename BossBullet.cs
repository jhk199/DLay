using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {
    
    
    public float speed;
    private Vector3 direction;
    public GameObject impact;
    private float dynamiccodex; // Slow bullets down
                                // Start is called before the first frame update

    void Start() {
        
        direction = transform.right;
        if (PlayerController.instance.dynamiccodex == true) {
            dynamiccodex = 3 / 4f;
        }
        else {
            dynamiccodex = 1f;
        }
    }

    // Update is called once per frame
    void Update() {
        transform.position += direction * (speed * dynamiccodex) * Time.deltaTime; // speed of bullet
        if(!BossScript.instance.gameObject.activeInHierarchy) {
            Destroy(gameObject);
        }
        if(CameraControllerNuclear.instance.boss == true) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") { // Get hit, take dmg
            PlayerHealthController.instance.damagePlayer();
            Destroy(gameObject);
        }
        else {
            Instantiate(impact, transform.position, transform.rotation); // Hit wall effect
            AudioManager.instance.playSfx(4);
            Destroy(gameObject);
        }


    }

    private void OnBecameInvisible() { // If shot off map
        Destroy(gameObject);
    }
}
