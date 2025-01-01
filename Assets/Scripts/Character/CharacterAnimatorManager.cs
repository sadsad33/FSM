using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    public CharacterManager character;
    public Animator animator;
    public bool disableOnAnimatorMove;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
        animator = GetComponent<Animator>();
    }

    public void OnAnimatorMove() {
        if (!character.isMoving && !character.isPerformingAction) return;
        if (disableOnAnimatorMove) return;
        Vector3 velocity = animator.deltaPosition;
        //Quaternion targetRotation = Quaternion.LookRotation(character.transform.forward);
        //Quaternion tr = Quaternion.Lerp(targetRotation, animator.deltaRotation, Time.deltaTime);
        //character.transform.rotation = tr;
        //character.transform.rotation = animator.deltaRotation;
        if (character.cc.enabled) character.cc.Move(velocity);
    }

    public virtual void PlayAnimation(string animation, bool isInteracting) {
        animator.applyRootMotion = isInteracting;
        //animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(animation, 0.2f);
    }
}
