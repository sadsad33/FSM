using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerMeleeJumpLightAttackState : PlayerAirborneActionState {
        
        Vector3 velocity;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(15f);
            player.consumingStamina = true;
            player.isAttacking = true;
            //if(player.isJumping)player.isJumping = false;
            if (player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() is MeleeWeaponItem) {
                MeleeWeaponItem meleeWeapon = player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() as MeleeWeaponItem;
                player.playerAnimatorManager.PlayAnimation(meleeWeapon.jumpAttackAnimation, player.isPerformingAction);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            HandleJumping();
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            //if (player.isGrounded)
            //    player.psm.ChangeState(player.psm.lightAttackLandingState);
            if (!player.isJumping) {
                player.psm.ChangeState(player.psm.fallingState);
            }
        }

        public void SetVelocity(Vector3 velocity) => this.velocity = velocity;

        public void HandleJumping() {
            if (player.cc.enabled) player.cc.Move(velocity * Time.deltaTime);
        }
    }
}