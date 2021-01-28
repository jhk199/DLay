using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject UI;

    private void Start() {
        UI = PlayerController.instance.GetComponentInChildren<UIController>().gameObject;
    }
    // Start is called before the first frame update
    void bossFocus() {
        PlayerController.instance.canMove = false;
        UI.SetActive(false);
        CameraControllerNuclear.instance.boss = true;
        mainCamera.orthographicSize = 5;
    }

    void playerFocus() {
        AudioManager.instance.win();
        PlayerController.instance.canMove = true;
        CameraControllerNuclear.instance.boss = false;
        mainCamera.orthographicSize = 8;
        UI.SetActive(true);
        mainCamera.GetComponent<CameraControllerNuclear>().enabled = false;
        mainCamera.GetComponent<CameraController>().enabled = true;
        BossScript.instance.levelExit.SetActive(true);
    }

    void spawned() {
        PlayerController.instance.canMove = true;
        CameraControllerNuclear.instance.boss = false;
        mainCamera.orthographicSize = 8;
        UI.SetActive(true);
        AudioManager.instance.level();
        BossScript.instance.spawned = true;
        BossScript.instance.anim.SetTrigger("go");
    }

    void deathNoise() {
        AudioManager.instance.playSfx(30);
    }

    void bossYell() {
        AudioManager.instance.playSfx(31);
    }

    void bossDeath() {
        AudioManager.instance.levelMusic.Stop();
        AudioManager.instance.playSfx(32);
    }

    void shoot() {
        AudioManager.instance.sfx[34].Stop();
        AudioManager.instance.playSfx(33);
        Debug.Log("Shoot");
    }

    void move() {
        AudioManager.instance.sfx[33].Stop();
        AudioManager.instance.playSfx(34);
        Debug.Log("Move");
    }
}
