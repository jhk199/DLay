using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnExit : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        animator.gameObject.SetActive(false);

        

    }
}

