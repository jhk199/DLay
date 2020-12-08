using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour {

    // Vanilla enemy: seeks out player and fires like mindless zombie at walls. Not as good as ASTAR enemy
    public static EnemyController instance;
    public Rigidbody2D theRB;
    public float moveSpeed;

    public float rangeToChase;
    private Vector3 moveDirection;
    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatter;
    public GameObject hitEffect;

    public bool shouldShoot;
    public bool isDead = false;
    public bool spotted = false;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shootRange;

    public SpriteRenderer body;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(body.isVisible && PlayerController.instance.gameObject.activeInHierarchy) {
            Move();
            shoot();
            pingDifficultyChange();
            
            
        }
        else {
            theRB.velocity = Vector2.zero;
        }
        
    }

    void Move() {
        // Debug.Log(Vector3.Distance(transform.position, PlayerController.instance.transform.position));
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChase) {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else {
            moveDirection = Vector3.zero;
        }

        moveDirection.Normalize();

        theRB.velocity = moveDirection * moveSpeed;

        if (theRB.velocity.x >= 0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (theRB.velocity.x <= -0.01f) {
            transform.localScale = Vector3.one;
        }
       

        if (moveDirection != Vector3.zero) {
            anim.SetBool("isMoving", true);
        }
        else {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage) {
        health -= damage;
        AudioManager.instance.playSfx(2);
        Instantiate(hitEffect, transform.position, transform.rotation);
        anim.SetTrigger("takeDmg");
        if(health <= 0) {
            anim.SetBool("isMoving", false);
            anim.SetBool("isDead", true);
            GetComponent<Collider2D>().enabled = false;
            theRB.velocity = Vector3.zero;
            this.enabled = false;
            int selectedSplat = Random.Range(0, deathSplatter.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatter[selectedSplat], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
        }
    }

    void shoot() {
        if(shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange) {
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0) {
                fireCounter = fireRate;
                AudioManager.instance.playSfx(16);
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }
    }

    void pingDifficultyChange() { // Enemies are faster the more ping the player has
        if (LevelManager.instance.currentPing > 0 && LevelManager.instance.currentPing < 50) {
            moveSpeed = 2f;
            fireRate = 1f;

        }
        else if (LevelManager.instance.currentPing >= 50 && LevelManager.instance.currentPing < 100) {
            moveSpeed = 3f;
            fireRate = .75f;
            
        }
        else if (LevelManager.instance.currentPing >= 100) {
            moveSpeed = 4f;
            fireRate = .5f;
            
        }
    }
}