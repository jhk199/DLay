using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunArray : MonoBehaviour {

    // To track guns for shops and chests
    public static GunArray instance;
    public List<GunPickup> gunList= new List<GunPickup>();
    
    public bool empty = false;
   
    // Start is called before the first frame update

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(gunList.Count == 0) {
            empty = true;
        }
       
    }
}
