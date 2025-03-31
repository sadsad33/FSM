using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterAnimatorManager : MonoBehaviour {
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
            if (character.cc.enabled) character.cc.Move(velocity);
        }

        public virtual void PlayAnimation(string animation, bool isInteracting) {
            animator.applyRootMotion = isInteracting;
            //animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(animation, 0.2f);
        }

        public void FinishAction() {
            character.isPerformingAction = false;
        }

        public void FinishAttack() {
            character.isAttacking = false;
        }

        public void FinishMovement() {
            character.isMoving = false;
        }

        public void EnableAttack() {
            character.canAttackDuringAction = true;
        }

        public void DisableAttack() {
            character.canAttackDuringAction = false;
        }

        public void EnableCombo() {
            character.canDoComboAttack = true;
        }

        public void DisableCombo() {
            character.canDoComboAttack = false;
        }

        public void EnableRotateDuringAction() {
            character.canRotateDuringAction = true;
        }

        public void DisableRotateDuringAction() {
            character.canRotateDuringAction = false;
        }
    }
}