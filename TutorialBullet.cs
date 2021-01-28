using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBullet : MonoBehaviour {
    // Start is called before the first frame update

    
    public float speed;
    private Vector3 direction;
    public GameObject impact;
    public enum directionShoot { up, down, left, right };
    public directionShoot shootDirection;
    void Start() {
        speed = 6;
        switch (shootDirection) {
            case directionShoot.up:
               
                direction = new Vector3(0, 1, 0);
                break;
            case directionShoot.down:
               
                direction = new Vector3(0, -1, 0);
                break;
            case directionShoot.left:
               
                direction = new Vector3(-1, 0, 0);
                break;
            case directionShoot.right:
               
                direction = new Vector3(1, 0, 0);
                break;
        }
           
    }

    // Update is called once per frame
    void Update() {
        transform.position += direction * (speed) * Time.deltaTime;
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
