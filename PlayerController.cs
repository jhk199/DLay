using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 99+ references, sheeeeeesh
public class PlayerController : MonoBehaviour {
    
    public static PlayerController instance;
    public float moveSpeed; // How fast you run
    public float activeMoveSpeed, moveSpeedPowerup = 1f; // How fast you currently are | Used by Fiber Optic Powerup
    private Vector2 moveInput; // Where you want to go
    // Hitbox
    public Rigidbody2D theRB;
    // Gun arm
    public Transform gunArm;
    // Powerup tracking
    public Transform powerupsStorage;
    // Animate yoself
    public Animator anim;
    // What should your body look like?
    public SpriteRenderer bodySR;
    // Gun to hold
    public GameObject gun;
    // Gun and Powerup lists
    public List<Gun> availableGuns = new List<Gun>();
    public List<Powerups> currentPowerups = new List<Powerups>();
    [HideInInspector]
    public int currentGun;
    // Switch weapons cd
    public float switchCD;
    private float switchCDcounter;
    private bool dodge = false; // Are you dodging?
    // Used by CloudFog
    public GameObject rating;
    public Text ratingText;
    // Roll cd based on enemies used by cloudLoop
    public float cloudLoopChange;
    // CloudFog timer
    public float cloudTimer;
    // dash
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvince = .5f;
    [HideInInspector]
    public float dashCounter;
    private float  dashCoolCounter;
    public GameObject dash;
    [HideInInspector]
    public bool canMove = true;
    public bool inCombat = false;
    // Powerup Bools
    [HideInInspector]
    public bool cloudfog = false, cloudgameloop = false, oneHop = false, shopVisited = false,
                deployserver = false, dynamiccodex = false, edgegame = false, hpc = false;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start() {
        activeMoveSpeed = moveSpeed * moveSpeedPowerup;
        AstarPath.active.Scan(); // Enemy tracking you now
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].gunName; 
    }

    // Update is called once per frame
    void Update() {
        if(canMove & !LevelManager.instance.isPaused) {
            if (cloudfog == true) { // If you have CloudFog powerup
                cloudFogTimer();
            }   
            Move();
            Aim();
            Dodge();
            gunInput();
        }
        else { // Stand still and breathe!
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);  
        }
    }

    void Move() {
        moveInput.x = Input.GetAxisRaw("Horizontal"); // Left right (A D)
        moveInput.y = Input.GetAxisRaw("Vertical"); // Up down (W S)
        moveInput.Normalize(); // No wonky math
        theRB.velocity = moveInput * activeMoveSpeed;
        // animations
        if (moveInput != Vector2.zero && dodge == false) {
            anim.SetBool("isMoving", true);
        }
        else {
            anim.SetBool("isMoving", false);   
        }
    }

    void Aim() {
        Vector3 mousePos = Input.mousePosition; // Aim where I point!
        Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
        // Rotate character
        if (mousePos.x < screenPoint.x) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            rating.transform.localScale = new Vector3(1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else {
            transform.localScale = Vector3.one;
            rating.transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = Vector3.one;
        }
        // rotate gun arm
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunArm.rotation = Quaternion.Euler(0, 0, angle);

    }
    
    void Dodge() {
        // Ignore game mechanics. Balanced
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(dashCoolCounter <= 0 && dashCounter <= 0) {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                AudioManager.instance.playSfx(9);
                anim.SetTrigger("dodge");
                // Leap over bullets!
                Physics2D.IgnoreLayerCollision(8, 11, true);
                Instantiate(dash, instance.transform.position, instance.transform.rotation);
                PlayerHealthController.instance.makeInvincible(dashInvince); // No damage for a bit
                dodge = false;
            } 
        }

        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0) {
                // Git hit by bullets again
                Physics2D.IgnoreLayerCollision(8, 11, false);
                activeMoveSpeed = moveSpeed * moveSpeedPowerup;
                dashCoolCounter = dashCooldown;
            }
        }
        // Dash timer
        if(dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    // Will be used eventually
    public void warp() {
        theRB.velocity = Vector2.zero;
        anim.SetTrigger("warp");
    }

    // Fire gun
    public void gunInput() {
        if (switchCDcounter > 0) {
            switchCDcounter -= Time.deltaTime;
        }
        else {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // Select gun with scroll up
                if (availableGuns.Count > 1) {
                    currentGun++;
                    if (currentGun >= availableGuns.Count) {
                        currentGun = 0;
                    }
                    switchGun();
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // Selectgun with scroll down
                if (availableGuns.Count > 1) {
                    currentGun--;
                    if (currentGun < 0) {
                        currentGun = availableGuns.Count - 1;
                    }
                    switchGun();
                }
            }
        }
        
        
    }

    public void switchGun() { // change current gun
        foreach(Gun theGun in availableGuns) {
            theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        switchCDcounter = switchCD;
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].gunName;
    }

    public void takeDamage() { // oW!
        anim.SetTrigger("playerTakeDmg");
    }

    void cloudFogTimer() { // Timer for cloudfog powerup
        cloudTimer += Time.deltaTime;
    }


}
