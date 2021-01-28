using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    
    public float speed = 7.5f;
    public Rigidbody2D theRB;
    public GameObject impact;
    public int damage = 0; 
    // Start is called before the first frame update

    
    void Start() {
        damage = Gun.instance.damage;    
    }

    // Update is called once per frame
    void Update() {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Instantiate(impact, transform.position, transform.rotation); // Hit effect
        AudioManager.instance.playSfx(4);
        Destroy(gameObject); // Get rid of bullet

        if(other.tag == "Enemy") {
            other.GetComponent<EnemyController>().DamageEnemy(damage); // For standard enemies
        }

        if (other.tag == "EnemyAstar") {
            other.GetComponent<EnemyControllerAstar>().DamageEnemy(damage); // For AI enemies
        }

        if(other.tag == "Boss") {
            BossScript.instance.takeDamage(damage);
            Instantiate(BossScript.instance.hitEffect, transform.position, transform.rotation);
        }

        if (other.tag == "Breakable") {
            other.GetComponent<Breakables>().objectBreak(); // For boxes
        }
        
    }

    private void OnBecameInvisible() { // If bullets are fired offscreen
        Destroy(gameObject);
    }
}
