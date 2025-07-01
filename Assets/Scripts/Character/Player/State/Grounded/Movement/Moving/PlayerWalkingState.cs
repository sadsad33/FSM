using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerWalkingState : PlayerMovingState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (player.pasm.GetCurrentState() != player.pasm.standingActionIdlingState) player.pasm.ChangeState(player.pasm.standingActionIdlingState);
            moveSpeedModifier = 0.6f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
            //HandleMovement();
        }

        public override void Exit(CharacterManager characer) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (moveAmount <= 0f) player.psm.ChangeState(player.psm.idlingState);
            else if (player.playerInputManager.CrouchInput && !player.isCrouching) {
                player.psm.standToCrouchState.tr = player.transform.position;
                player.psm.ChangeState(player.psm.standToCrouchState);
            } else if (!player.playerInputManager.WalkInput) {
                player.psm.ChangeState(player.psm.runningState);
            }
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
            //currentMovingSpeed = speed;
            moveDirection *= speed;
            //if (moveDirection.magnitude > CharacterMaximumVelocity.magnitude)
            //    CharacterMaximumVelocity = moveDirection;
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}