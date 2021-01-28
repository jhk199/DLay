using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyControllerAstar : MonoBehaviour {

    // This is the ASTAR controlled enemy. Powered by pathfinding AI
    public static EnemyControllerAstar instance;
    
    public Rigidbody2D theRB;
    // public float moveSpeed;

    public bool stopShoot;
    public float rangeToChase, rangeToStop;
    // private Vector3 moveDirection;
    public Animator anim;
    // Set HP
    public int health = 150;

    public GameObject[] deathSplatter;
    public GameObject[] russianBullets;
    public GameObject hitEffect;

    public bool shouldShoot;
    public float canShootTimer = 1;
    public bool canShoot = true;

    
    
    // Shooting
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    public float lowFireRate;
    public float mediumFireRate;
    public float highFireRate;
    public float fireCounter;
    public float shootRange;
    public float bulletSpeed;
    public bool russian;

    public SpriteRenderer body;

    // AI STUFF
    
    public float speed = 400f;
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath;
    Seeker seeker;

    // item drop
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    private void Awake() {
        instance = this;
    }
    
    void Start() { // Start AI
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update() {
        // Can the enemy see the player?
        if (body.isVisible && PlayerController.instance.gameObject.activeInHierarchy) {
            Move();
            sightTest();
            shootTest();
            if(canShoot) {
               shoot();
            }
        }
        else { // Stand still
            theRB.velocity = Vector2.zero;
        }
        // Pathfinding
        if (path == null) {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        // Change directions
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - theRB.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        theRB.AddForce(force);

        float distance = Vector2.Distance(theRB.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
        if (theRB.velocity.x >= 0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (theRB.velocity.x <= 0.01f) {
            transform.localScale = Vector3.one;
        }

    }

    // More AI
    void UpdatePath() {
        if (seeker.IsDone())
            seeker.StartPath(theRB.position, PlayerController.instance.transform.position, onPathComplete);
    }

    void onPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Enemy movement
    void Move() {
        // Set active if close
        if ((Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChase) 
            && (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToStop)) {
            
            speed = 400f;
            theRB.drag = 2f;
            shouldShoot = true;
            anim.SetBool("isMoving", true);
        }
        else {
            speed = 0f;
            theRB.drag = 1000f; // Slow down cowboy!
            anim.SetBool("isMoving", false);
            
        }
    }

    // Hurt enemy until dead
    public void DamageEnemy(int damage) {
        health -= damage;
        speed = 0f;
        theRB.drag = 1000f;
        AudioManager.instance.playSfx(2);
        Instantiate(hitEffect, transform.position, transform.rotation);
        anim.SetTrigger("takeDmg");
        if (health <= 0) {
            anim.SetBool("isMoving", false);
            anim.SetBool("isDead", true);
            GetComponent<Collider2D>().enabled = false; // Disable collider
            theRB.velocity = Vector3.zero;
            this.enabled = false;
            int selectedSplat = Random.Range(0, deathSplatter.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatter[selectedSplat], transform.position, Quaternion.Euler(0f, 0f, rotation * 90)); // Death splatters, yum
            if (shouldDropItem) { // Should the enemy drop items?
                float dropChance = Random.Range(0, 100f); // Random chance
                bool hasDropped = false;
                if (dropChance < itemDropPercent && hasDropped == false) {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[0], transform.position, transform.rotation);
                    hasDropped = true;
                }
            }
        }
    }

    void shoot() { // Bang bang, fire bullets 
        if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange) {
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0) {
                fireCounter = fireRate;
                pingDifficultyChange();
                AudioManager.instance.playSfx(16);
                if(!russian) {
                    Instantiate(bullet, firePoint.position, firePoint.rotation); // Mkae bullet, send out bullet
                }
                else {
                    theRB.velocity = Vector2.zero;
                    anim.SetTrigger("shoot");
                    int rand = Random.Range(0, russianBullets.Length);
                    Instantiate(russianBullets[rand], transform.position, transform.rotation);
                }
                

            }
            
        }
        
    }

    // Line of sight function by using Tag colliders and drawing Rays
    void sightTest() {
        Vector3 start = firePoint.position;
        Vector3 direction = (PlayerController.instance.transform.position - start).normalized;
        float distance = Vector3.Distance(start, PlayerController.instance.transform.position);
        Debug.DrawRay(start, direction * distance, Color.red);
        RaycastHit2D[] sightTestResults = Physics2D.RaycastAll(start, direction, distance); // Draw red line
        for (int i = 0; i < sightTestResults.Length; i++) {
            RaycastHit2D sightTest = sightTestResults[i];
            if (sightTest.collider.tag == "Wall") {
                shouldShoot = false;
                break;
            }
            else {
                shouldShoot = true; // Blast away!
            }
        }
    }
    void pingDifficultyChange() { // Enemies are faster the more ping the player has
        if (LevelManager.instance.currentPing > 0 && LevelManager.instance.currentPing < 50) {
            speed = 400f;
            fireRate = lowFireRate;
            bulletSpeed = 6f;
        }
        else if (LevelManager.instance.currentPing >= 50 && LevelManager.instance.currentPing < 100) {
            speed = 500f;
            fireRate = mediumFireRate;
            bulletSpeed = 7.5f;
        }
        else if (LevelManager.instance.currentPing >= 100) {
            speed = 650f;
            fireRate = highFireRate;
            bulletSpeed = 9f;
        }
    }

    

    void shootTest() {
        canShootTimer += Time.deltaTime;
        if (canShootTimer > 2) {
            canShoot = true;
        }
        else {
            canShoot = false;
        }
    }

    

   
}