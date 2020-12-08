using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour {

    // Function for breakable pieces
    public float moveSpeed = 3f;
    private Vector3 moveDirection;
    public float deceleration = 5f;
    public float lifetime = 3f;
    // Start is called before the first frame update
    void Start() {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update() { // Gives pieces random spin and deletes after fade out
        transform.position += moveDirection * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if(lifetime < -10) {
            Color color = this.GetComponent<Renderer>().material.color;
            float fadeAmount = color.a - (0.5f * Time.deltaTime);
            color = new Color(color.r, color.g, color.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = color;
            if(color.a <= 0) {
                Destroy(gameObject);
            }
            
        }
    }
}
