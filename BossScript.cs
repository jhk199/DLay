using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {
    // Start is called before the first frame update

    public static BossScript instance;

    
    private int currentAction;
    private float actionCounter;
    private float shotCounter;
    private Vector2 moveDirection;
    public Rigidbody2D theRB;
    public Animator anim;
    public bool spawned = false;
    public int currentHealth;

    public GameObject levelExit;
    public GameObject hitEffect;
    public BossAction[] actions;
    public BossSequence[] sequences;
    public int currentSequence;

    private void Awake() {
        instance = this;
    }
    void Start() {
        if(CharacterTracker.instance.ping > 100) {
            currentHealth = 4000;
        }
        actions = sequences[currentSequence].actions;
        actionCounter = actions[currentAction].actionLength;
        UIController.instance.bossHealthBar.gameObject.SetActive(true);
        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update() {
        if(spawned == true) {
            actionLoop();
        }
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            
            anim.SetBool("isMoving", false);
            anim.SetBool("isDead", true);
            GetComponent<Collider2D>().enabled = false; // Disable collider
            theRB.velocity = Vector3.zero;
            enabled = false;
            GetComponent<Collider2D>().enabled = false;
            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f) {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }
            
            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        else {
            if(currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1) {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    void actionLoop() {
        if (actionCounter > 0) {
            actionCounter -= Time.deltaTime;
            moveDirection = Vector2.zero;
            anim.SetBool("isMoving", false);
            if (actions[currentAction].shouldMove) {
                if (actions[currentAction].shouldChase) {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                    AudioManager.instance.sfx[33].Stop();
                    anim.SetBool("isMoving", true);
                }
                if (actions[currentAction].moveToPoints) {
                    moveDirection = actions[currentAction].pointToMove.position - transform.position;
                    moveDirection.Normalize();
                }
            }
            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;

            if (actions[currentAction].shouldShoot) {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0 ) {
                    AudioManager.instance.sfx[34].Stop();
                    anim.SetTrigger("shoot");
                    shotCounter = actions[currentAction].timeBetween;
                    foreach (Transform t in actions[currentAction].shotPoints) {
                        Instantiate(actions[currentAction].shootObject, t.position, t.rotation);
                    }
                }
            }

        }
        else {
            currentAction++;
            if (currentAction >= actions.Length) {
                currentAction = 0;
            }
            actionCounter = actions[currentAction].actionLength;
        }
    }

    
}

[System.Serializable]
public class BossAction {
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChase;
    public float moveSpeed;

    public bool moveToPoints;

    public Transform pointToMove;

    public bool shouldShoot;
    public GameObject shootObject;
    public float timeBetween;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence {
    [Header("Sequence")]
    public BossAction[] actions;

    public float endSequenceHealth;
    
    void Start() {
        if (PlayerController.instance.currentPowerups.Count > 5) {
            endSequenceHealth *= 1.5f;

        }
    }
}
