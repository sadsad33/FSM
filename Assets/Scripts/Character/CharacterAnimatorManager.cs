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

        public virtual void OnAnimatorMove() {
            if (!character.isMoving && !character.isPerformingAction) return;
            if (disableOnAnimatorMove) return;
            //Debug.Log("루트모션으로 제어");

            //Vector3 velocity = animator.deltaPosition / Time.deltaTime;
            Vector3 deltaPosition = animator.deltaPosition;
            Vector3 velocity = deltaPosition / Time.deltaTime; 
            if (character.cc.enabled) {
                character.cc.Move(velocity * Time.deltaTime);
                //character.transform.rotation *= animator.deltaRotation;
            }
        }

        public virtual void PlayAnimation(string animation, bool isInteracting) {
            animator.applyRootMotion = isInteracting;
            //animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(animation, 0.2f);
        }

        #region 애니메이션 이벤트
        public void FinishAction() {
            //Debug.Log("호출");
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

        public void BeVulnerable() {
            character.isInvulnerable = false;
        }

        public void EnableParrying() {
            character.isParrying = true;
        }

        public void DisableParrying() {
            character.isParrying = false;
        }

        public void EnableRiposte() {
            character.canBeRiposted = true;
        }

        public void DisableRiposte() {
            character.canBeRiposted = false;
        }

        #endregion
    }
}