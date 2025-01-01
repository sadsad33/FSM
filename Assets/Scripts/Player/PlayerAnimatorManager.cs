using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerAnimatorManager : CharacterAnimatorManager {
        public PlayerManager player;
        
        protected override void Awake() {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void PlayAnimation(string animation, bool isInteracting) {
            base.PlayAnimation(animation, isInteracting);
        }

        public void FinishAction() {
            player.isPerformingAction = false;
        }

        public void FinishAttack() {
            player.isAttacking = false;
        }

        public void FinishMovement() {
            player.isMoving = false;
        }

        public void EnableAttack() {
            player.canAttackDuringAction = true;
        }

        public void DisableAttack() {
            player.canAttackDuringAction = false;
        }

        public void EnableCombo() {
            player.canDoComboAttack = true;
        }

        public void DisableCombo() {
            player.canDoComboAttack = false;
        }

        public void EnableRotateDuringAction() {
            player.canRotateDuringAction = true;
        }

        public void DisableRotateDuringAction() {
            player.canRotateDuringAction = false;
        }
    }
}
