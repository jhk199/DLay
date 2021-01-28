using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {
    
    public bool russian, blob, dataman, skelly;
    void playDeath() {
        if (russian) {
            AudioManager.instance.playSfx(37);
        }
        if (skelly) {
            AudioManager.instance.playSfx(35);
        }
        if (blob) {
            AudioManager.instance.playSfx(36);
        }
        if (dataman) {
            AudioManager.instance.playSfx(38);
        }
    }

    void playMove() {
        
        if (skelly) {
            AudioManager.instance.playSfx(40);
        }
        if (blob) {
            AudioManager.instance.playSfx(39);
        }
        
    }
}
