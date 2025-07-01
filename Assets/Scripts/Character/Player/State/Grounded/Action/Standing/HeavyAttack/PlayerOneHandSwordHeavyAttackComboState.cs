using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerOneHandSwordHeavyAttackComboState : PlayerStandingAttackActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(30f);
            player.consumingStamina = true;
            player.canDoComboAttack = false;
            player.isPerformingAction = true;
            player.isAttacking = true;
            player.playerAnimatorManager.PlayAnimation("OH_Sword_HeavyAttack2", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            HandleRotation();
        }

        public override void Exit(CharacterManager character) {
            //player.isPerformingAction = false;
            player.consumingStamina = false;
            player.isAttacking = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction)
                player.psm.ChangeState(player.psm.idlingState);
        }

        private void HandleRotation() {
            if (!player.canRotateDuringAction) return;
            lookingDirection = CameraManager.instance.cameraTransform.forward * verticalInput;
            lookingDirection += CameraManager.instance.cameraTransform.right * horizontalInput;
            lookingDirection.Normalize();
            lookingDirection.y = 0;

            if (lookingDirection == Vector3.zero) {
                lookingDirection = player.transform.forward;
            }
            Quaternion tr = Quaternion.LookRotation(lookingDirection);
            Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
            player.transform.rotation = targetRotation;
        }
    }
}