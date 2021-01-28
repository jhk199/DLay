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
    private bool hpcChange = false, gcloudChange = false;
    public int damage;

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
        if(hpcChange == false && PlayerController.instance.hpc == true) {
            if(isShotgun) {
                spreadMax -= 10;
                spreadMin += 10;
                
            }
            else {
                spreadMax -= 2;
                spreadMin += 2;
                
            }
            hpcChange = true;
        }
        if (gcloudChange == false && PlayerController.instance.gcloud == true) {
            spreadMax += 5;
            spreadMin -= 5;
            damage /= 5;
            gcloudChange = true;
        }
    }

    void Shoot() {
        if(shotCounter > 0) {
            shotCounter -= Time.deltaTime;
        }
        else {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && PlayerController.instance.gcloud == false)  {
                AudioManager.instance.playSfx(gunSound);       
                if(isShotgun) { // Make LOTTA BULLETS FOR SHOTGUN!!!!
                    makeBullet(8);
                }
                else { // Or one lame single bullet
                    makeBullet(1);
                } 
                shotCounter = timeShot * timeShotPowerup; // Set shot counter based on timeShot and if you have fun powerup
            } 
            else if((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))  {
                AudioManager.instance.playSfx(gunSound);
                if (isShotgun) { // Make LOTTA BULLETS FOR SHOTGUN!!!!
                    makeBullet(40);
                    
                }
                else { // Or one lame single bullet
                    makeBullet(5);
                    
                }
                shotCounter = timeShot * timeShotPowerup; // Set shot counter based on timeShot and if you have fun powerup
            }
        }
    }

    void makeBullet(int times) {
        int i = 0;
        while(i < times) {
            
            Instantiate(bulletToFire, firePoint.position, firePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(spreadMin, spreadMax)));
            i++;
        }
    }
}
