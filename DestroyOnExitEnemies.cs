using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExitEnemies : StateMachineBehaviour {

    // Simple script to remove game objects once their animation has finished
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Destroy(animator.transform.parent.gameObject, stateInfo.length);

    }
}