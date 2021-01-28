using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerNuclear : MonoBehaviour {
    // Start is called before the first frame update
    public static CameraControllerNuclear instance;
    public Transform Player;
    public Transform Boss;
    public bool boss = false;

    Vector3 target, mousePos, refVel;
    float cameraDist = 3.5f;
    float smoothTime = 0.2f, zStart;

    private void Awake() {
        instance = this;
    }
    void Start() {
        Player = PlayerController.instance.transform;
        target = Player.position;
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void Update() {
        
        if(boss == true) {
            target = UpdateBossPos();
        }
        
        if(boss == false) {
            mousePos = CaptureMousePos();
            target = UpdateTargetPos();
        }
        UpdateCameraPosition();

    }

    Vector3 CaptureMousePos() {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if(Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max) {
            ret = ret.normalized;
        }
        return ret;
    }

    Vector3 UpdateTargetPos() {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = Player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }

    Vector3 UpdateBossPos() {
        Vector3 ret = Boss.position;
        ret.z = zStart;
        return ret;
    }



    void UpdateCameraPosition() {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }
}
