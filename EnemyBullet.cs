using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public static EnemyBullet instance;
    public float speed;
    private Vector3 direction;
    public GameObject impact;
    private float dynamiccodex; // Slow bullets down
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start() {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        if(PlayerController.instance.dynamiccodex == true) {
            dynamiccodex = 3 / 4f;
        }
        else {
            dynamiccodex = 1f;
        }
    }

    // Update is called once per frame
    void Update() {
        transform.position += direction * (speed * dynamiccodex) * Time.deltaTime; // speed of bullet
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") { // Get hit, take dmg
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
