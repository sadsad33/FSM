using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerSlideAttackState : PlayerGroundedActionState {

        Vector3 direction;
        float speed;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //player.pmsm.ChangeState(player.pmsm.notMovingState);
            player.isAttacking = true;
            if (player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() is MeleeWeaponItem) {
                MeleeWeaponItem meleeWeapon = player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() as MeleeWeaponItem;
                player.playerAnimatorManager.PlayAnimation(meleeWeapon.slidAttackAnimation, player.isPerformingAction);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            float speedDecayRate = 5f; 
            speed = Mathf.MoveTowards(speed, 0f, speedDecayRate * Time.deltaTime);
            HandleSliding();
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            //player.pmsm.ChangeState(player.pmsm.idlingState);
            player.isMoving = false;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isAttacking)
                player.psm.ChangeState(player.psm.idlingState);
        }

        public void HandleSliding() {
            if (player.cc.enabled) player.cc.Move(speed * Time.deltaTime * direction);
        }

        public void SetVelocity(Vector3 direction, float speed) {
            this.direction = direction;
            this.speed = speed;
        }
    }
}