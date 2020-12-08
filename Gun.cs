using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public static Gun instance;
    public GameObject bulletToFire;
    public Transform firePoint;
    public float timeShot, timeShotPowerup = 1f; // Shoot quicker
    private float shotCounter;
    public bool isShotgun; // To create more bullets
    public int spreadMin, spreadMax; // Random spread
    public int gunSound; // bang bang
    public string gunName;
    public Sprite gunUI;

    public int itemCost; // $$$$$
    public Sprite gunShopSprite;


    private void Awake() {
        instance = this;
    }
    
    void Start() {
        
    }
    void Update() {
        if(PlayerController.instance.canMove && !LevelManager.instance.isPaused) {
            Shoot();
        }
    }

    void Shoot() {
        if(shotCounter > 0) {
            shotCounter -= Time.deltaTime;
        }
        else {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
                AudioManager.instance.playSfx(gunSound);
                if(isShotgun) { // Make LOTTA BULLETS FOR SHOTGUN!!!!
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));

                }
                else { // Or one lame single bullet
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
                } 
                shotCounter = timeShot * timeShotPowerup; // Set shot counter based on timeShot and if you have fun powerup
            }  
        }
    }
}
