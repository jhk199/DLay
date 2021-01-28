using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShooter : MonoBehaviour {
    // Start is called before the first frame update

    public static TutorialShooter instance;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public bool shouldShoot, canShoot;
    public float canShootTimer;
    public SpriteRenderer body;
    public Vector3 direction;
    


    private void Awake() {
        instance = this;
    }
    void Start() {
        canShootTimer = 2f;
    }

    // Update is called once per frame
    void Update() {
        if (body.isVisible && PlayerController.instance.gameObject.activeInHierarchy) {
            shootTest();
            if(canShoot) {
                shoot();
            }
            
            
        }
    }

    void shoot() {
        if (shouldShoot) {
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0) {
                fireCounter = fireRate;
                AudioManager.instance.playSfx(16);
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
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
