using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerCrouchWalkState : PlayerMovingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            moveSpeedModifier = 0.3f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.playerInputManager.CrouchInput) player.psm.ChangeState(player.psm.crouchToStandState);
            else if (moveAmount <= 0f) player.psm.ChangeState(player.psm.crouchIdleState);
        }

        protected override void HandleRotation() {
            base.HandleRotation();
            Quaternion tr = Quaternion.LookRotation(lookingDirection);
            Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
            player.transform.rotation = targetRotation;
        }

        protected override void HandleMovement() {
            base.HandleMovement();
            float speed = player.moveSpeed * moveSpeedModifier;
            moveDirection *= speed;
            //if (moveDirection.magnitude > CharacterMaximumVelocity.magnitude)
            //    CharacterMaximumVelocity = moveDirection;
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}