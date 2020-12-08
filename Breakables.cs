using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour {

    public GameObject[] brokenPieces; // Cool effects
    public int maxPieces = 10;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public int breakSound = 0;
    

    void Start() {
        
    }

    
    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D other) { // If player dashes into box
        if (other.gameObject.tag == "Player") {
            if (PlayerController.instance.dashCounter > 0) {
                objectBreak();

            }
        }
    }



    public void objectBreak() { // Break the object!
        Destroy(gameObject);
        AudioManager.instance.playSfx(breakSound);
        // show broken pieces
        int piecesToDrop = Random.Range(1, maxPieces); // Create broken shards
        for (int i = 0; i < piecesToDrop; i++) {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            int rotation = Random.Range(0, 360);
            Instantiate(brokenPieces[randomPiece], transform.position, Quaternion.Euler(0f, 0f, rotation));
        }
        // Drop items
        if(shouldDropItem) { // Make item
            float dropChance = Random.Range(0, 100f);
            if(dropChance < itemDropPercent) {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }

}
