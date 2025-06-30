using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerOneHandSwordFirstAttackState : PlayerStandingAttackActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(20f);
            player.consumingStamina = true;
            player.isAttacking = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("OH_Sword_Attack1", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            HandleComboAttack();
            HandleRotation();
        }

        public override void Exit(CharacterManager character) {
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
            player.isAttacking = false;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
            }
        }

        private void HandleComboAttack() {
            if (player.canDoComboAttack && player.playerStatsManager.currentStamina >= 10f) {
                if (player.playerInputManager.LightAttackInput)
                    player.pasm.ChangeState(player.pasm.oneHandSwordComboAttackState);
                else if (player.playerInputManager.HeavyAttackInput)
                    player.pasm.ChangeState(player.pasm.oneHandSwordHeavyAttackState);
            }
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