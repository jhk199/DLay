using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Play pretty noises when told to by Unity
    public static AudioManager instance;
    public AudioSource levelMusic, gameOverMusic, winMusic;
    public AudioSource[] sfx;
    private void Awake() {
        instance = this;
    }

    void Start() {
        
    }

    void Update() {
        
    }

    public void gameOver() {
        levelMusic.Stop();
        gameOverMusic.Play();
    }
    public void win() {
        levelMusic.Stop();
        winMusic.Play();
    }

    

    public void level() {
        winMusic.Stop();
        levelMusic.Play();
    }


    public void playSfx(int sfxPlay) {
        sfx[sfxPlay].Stop();
        sfx[sfxPlay].Play();
    }
}
