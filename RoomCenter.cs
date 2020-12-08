using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour {
    
    public List<GameObject> enemies = new List<GameObject>(); // Enemies in room
    public bool openWhenEnemiesClear; // Does the room need to close it's doors?
    public Room theRoom; // The Room the center is occupying

    // Start is called before the first frame update
    void Start() {
        if(openWhenEnemiesClear) { // Close doors
            theRoom.doorsCloseOnEnter = true;   
        }  
    }

    // Update is called once per frame
    void Update() {
        if (!(theRoom.PlayerIsHere() && openWhenEnemiesClear)) {
            return; // Ignore if Player is absent
        }
        
        for (int i = 0; i < enemies.Count; i++) {
            if (PlayerController.instance.currentPowerups.Count != 0) {
                if (PlayerController.instance.currentPowerups.Count != 0) {
                    Powerups.instance.enemiesInRoom = enemies.Count; // Tracking for enemy count-based powerups
                }
            }
            if (enemies[i] == null) {
                enemies.RemoveAt(i);
                i--;
                if (PlayerController.instance.currentPowerups.Count != 0) {
                    Powerups.instance.enemiesInRoom--; // Tracking for enemy count-based powerups
                }
            }
        }

        if (enemies.Count == 0) {
            theRoom.removeDoors();
            if (PlayerController.instance.currentPowerups.Count != 0) {
                Powerups.instance.enemiesInRoom = 0; // Tracking for enemy count-based powerups
            }
        }
    }
}
