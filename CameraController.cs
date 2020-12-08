using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;
    public Camera mainCamera, mapCam, bigMapCam;
    public float moveSpeed;
    public Transform target;
    public bool miniMapEnabled;
    public bool mapActive;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update() {
        if(target != null) {
            // Update player position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
        miniMapZoom();
        miniMapEnable();
        inCombatMiniMap();
        mapToggle();
        
    }

    public void changeTarget(Transform newTarget) { //Used in Room
        target = newTarget;
    }

    public void miniMapZoom() { // Minus key zooms out
        if (Input.GetKeyDown(KeyCode.Minus) && mapCam.orthographicSize < 50f) {
            mapCam.orthographicSize += 10f;
            
        }
        else if(Input.GetKeyDown(KeyCode.Equals) && mapCam.orthographicSize > 10f) { // Plus zooms in
            mapCam.orthographicSize -= 10f;
            
        }
    }

    public void miniMapEnable() { // Minimap toggle
        // Disabled
        if(miniMapEnabled && Input.GetKeyDown(KeyCode.N) && (!mapActive)) {
            mapCam.backgroundColor = Color.clear;
            mapCam.cullingMask = 0;
            miniMapEnabled = false;
            UIController.instance.minimapText.SetActive(false);
        }
        // Enabled
        else if ((!miniMapEnabled && Input.GetKeyDown(KeyCode.N) && (!mapActive)) ) {
            mapCam.backgroundColor = Color.black;
            mapCam.cullingMask = 32768;
            miniMapEnabled = true;
            UIController.instance.minimapText.SetActive(true);
        }
    }

    public void inCombatMiniMap() { // Disables minimap if in combat. Keeps track if player had map disabled already
        if ((PlayerController.instance.inCombat == false) && (miniMapEnabled == true) && (!mapActive)) {
            mapCam.backgroundColor = Color.black;
            mapCam.cullingMask = 32768;
            miniMapEnabled = true;
            UIController.instance.minimapText.SetActive(true);
        }
        else if ((PlayerController.instance.inCombat == true) && (miniMapEnabled == true)) {
            mapCam.backgroundColor = Color.clear;
            mapCam.cullingMask = 0;
            UIController.instance.minimapText.SetActive(false);
        }
    }
    
    public void mapToggle() { // Function for Fullscreen map
        if(!LevelManager.instance.isPaused) {
            if (mapActive && Input.GetKeyDown(KeyCode.M)) {  // off
                bigMapCam.enabled = false;
                mapActive = false;
                if (miniMapEnabled == true) {
                    mapCam.backgroundColor = Color.black;
                    mapCam.cullingMask = 32768;
                    UIController.instance.minimapText.SetActive(true);
                }
                UIController.instance.bigMapText.SetActive(false);
                
                Time.timeScale = 1f;
                PlayerController.instance.canMove = true;
            }
            else if ((!mapActive && Input.GetKeyDown(KeyCode.M))) { // on
                if (miniMapEnabled == true) {
                    mapCam.backgroundColor = Color.clear;
                    mapCam.cullingMask = 0;
                }
                bigMapCam.enabled = true;
                mapActive = true;
                UIController.instance.bigMapText.SetActive(true);
                UIController.instance.minimapText.SetActive(false);
                Time.timeScale = 0f;
                PlayerController.instance.canMove = false;
            }
        }
    }
}
