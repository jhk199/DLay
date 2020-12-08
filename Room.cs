using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    public bool doorsCloseOnEnter;
    public GameObject[] doors;
    public GameObject mapHider, outline;
    public int index; // tracking for Cloudlet Powerup
   
    void Update() {
      
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) { // Ignore if not player
            return;
        }
        // If statement for Cloudlet Powerup
        if (PlayerController.instance.oneHop == true && index != LevelGenerator.instance.generatedOutlines.Count - 1) {
            LevelGenerator.instance.generatedOutlines[index + 1].GetComponentInChildren<Room>().outline.SetActive(true);
            LevelGenerator.instance.generatedOutlines[index + 1].GetComponentInChildren<Room>().mapHider.SetActive(false);
        }
       
        LevelManager.instance.CurrentRoom = this;
        // Minimap changes
        outline.SetActive(true);
        mapHider.SetActive(false);
        moveCameraToHere();
        MaybeActivateDoors();
    }

    private void MaybeActivateDoors() { // If activate doors on enemy kill is true
        if (!doorsCloseOnEnter) {
            return; // If false, do nothing
        }

        foreach (var door in doors) {
            door.SetActive(true);
            // This closes doors
        }

        PlayerController.instance.inCombat = true; // For Hiding Minimap
       
    }

    private void moveCameraToHere() {
        CameraController.instance.changeTarget(transform); // Move camera
        AstarPath.active.Scan(); // Update AI pathing for enemies
        
    }

    public void removeDoors() { // Removes doors
        foreach (var door in doors) {
            door.SetActive(false);
        }
        doorsCloseOnEnter = false;
        PlayerController.instance.inCombat = false; // Minimap displays again
    }

    [HideInInspector]
    public bool PlayerIsHere() { // Player tracking for minimap
        
        return LevelManager.instance.CurrentRoom == this; 
    }
}