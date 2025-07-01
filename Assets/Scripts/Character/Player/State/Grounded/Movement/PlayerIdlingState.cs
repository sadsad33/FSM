using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerIdlingState : PlayerGroundedState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isMoving = false;
            //player.isPerformingAction = false;
            if(player.pasm.GetCurrentState() != player.pasm.standingActionIdlingState)
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
            player.RunningStateTimer = 0f;
            moveSpeedModifier = 0f;
            player.playerInputManager.SprintInputTimer = 0f;
            if (!sprintInputDelaySet)
                sprintInputDelaySet = true;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (player.cc.enabled)
                player.cc.Move(Vector3.zero);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
            //HandleMovement();
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.playerInputManager.CrouchInput) {
                player.psm.standToCrouchState.tr = player.transform.position;
                player.psm.ChangeState(player.psm.standToCrouchState);
            } else if (player.playerInputManager.JumpInput) {
                player.psm.ChangeState(player.psm.standingJumpState);
            } else if (moveAmount > 0f) {
                if (player.playerInputManager.WalkInput) {
                    player.psm.ChangeState(player.psm.walkingState);
                } else {
                    player.psm.ChangeState(player.psm.runningState);
                }
            }
        }

        protected override void HandleMovement() {
            base.HandleMovement();
            //float speed = player.moveSpeed * moveSpeedModifier;
            ////currentMovingSpeed = speed;
            //moveDirection *= speed;
            ////if (moveDirection.magnitude > CharacterMaximumVelocity.magnitude)
            ////    CharacterMaximumVelocity = moveDirection;
            //if (player.cc.enabled)
            //    player.cc.Move(moveDirection * speed);
        }
    }
}