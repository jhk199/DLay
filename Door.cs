using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    // Start is called before the first frame update
    public Animator anim;
    public static Door instance;
    void Start() {
        
    }
    private void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Close() {
        anim.SetTrigger("doorOpen");
        

    }

    void Disable() {
        Destroy(anim.gameObject);
    }

    
}
