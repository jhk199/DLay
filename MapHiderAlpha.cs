using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHiderAlpha : MonoBehaviour {

    
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start() {
        
        rawImage.color = new Color(1f, 1f, 1f, .68f);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
