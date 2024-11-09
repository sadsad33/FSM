using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    public CharacterManager character;
    public Animator animator;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
        animator = GetComponent<Animator>();
    }

    public void OnAnimatorMove() {
        if (!character.isMoving && !character.isPerformingAction) return;
        Vector3 velocity = animator.deltaPosition;
        if (character.cc.enabled) character.cc.Move(velocity);
    }

    public virtual void PlayAnimation(string animation, bool isInteracting) {
        animator.applyRootMotion = isInteracting;
        //animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(animation, 0.2f);
    }
}
