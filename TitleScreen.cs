using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { // Makes sure Player is destroyed on main menu
        if(GameObject.Find("UI Canvas")) {
            Destroy(UIController.instance.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
