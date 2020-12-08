using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupArray : MonoBehaviour {
    // Like gun array!
    public static PowerupArray instance;
    public List<PowerupsPickup> powerupList = new List<PowerupsPickup>();
    public bool empty = false;
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (powerupList.Count == 0) {
            empty = true;
        }

    }
}