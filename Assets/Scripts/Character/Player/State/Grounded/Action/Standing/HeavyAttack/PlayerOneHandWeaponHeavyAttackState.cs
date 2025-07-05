using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerOneHandWeaponHeavyAttackState : PlayerStandingAttackActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(30f);
            player.consumingStamina = true;
            player.canDoComboAttack = false;
            player.isPerformingAction = true;
            player.isAttacking = true;
            if (player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() is MeleeWeaponItem) {
                MeleeWeaponItem meleeWeapon = player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() as MeleeWeaponItem;
                player.playerAnimatorManager.PlayAnimation(meleeWeapon.heavyAttackAnimations[0], player.isPerformingAction);
            }
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
            if (!player.isPerformingAction)
                player.psm.ChangeState(player.psm.idlingState);
        }

        private void HandleComboAttack() {
            if (player.canDoComboAttack && player.playerStatsManager.currentStamina >= 15f) {
                if (player.playerInputManager.HeavyAttackInput)
                    player.psm.ChangeState(player.psm.oneHandWeaponHeavyAttackComboState);
                else if (player.playerInputManager.LightAttackInput)
                    player.psm.ChangeState(player.psm.oneHandWeaponLightComboAttackState);
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