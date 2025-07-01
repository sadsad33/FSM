using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRunningState : PlayerMovingState {

        private float canSlidingDelayTimer;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (player.pasm.GetCurrentState() != player.pasm.standingActionIdlingState) player.pasm.ChangeState(player.pasm.standingActionIdlingState);
            player.playerInputManager.SprintInputTimer = 0f;
            if (!sprintInputDelaySet)
                sprintInputDelaySet = true;
            moveSpeedModifier = 1f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            canSlidingDelayTimer += Time.deltaTime;
            if (canSlidingDelayTimer > 0.3f) {
                player.canSliding = false;
            }
            //Debug.Log("Running State Stay ÀÇ MoveDirection : " + moveDirection);
            player.RunningStateTimer += Time.deltaTime;
            //HandleMovement();
            player.playerAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            canSlidingDelayTimer = 0f;
        }


        public override void HandleInput() {
            base.HandleInput();
            if (moveAmount <= 0f) {
                if (player.RunningStateTimer >= 0.5f)
                    player.psm.ChangeState(player.psm.lightStoppingState);
                else
                    player.psm.ChangeState(player.psm.idlingState);
            } else if (!player.isPerformingAction) {
                if (player.playerInputManager.JumpInput && player.playerStatsManager.currentStamina > 10f) {
                    player.psm.runningJumpState.MovingVelocityInAir = moveDirection;
                    player.psm.ChangeState(player.psm.runningJumpState);
                } else if (player.playerInputManager.CrouchInput) {
                    if (player.canSliding)
                        player.psm.ChangeState(player.psm.slidingState);
                    else if (!player.isCrouching) {
                        player.psm.standToCrouchState.tr = player.transform.position;
                        player.psm.ChangeState(player.psm.standToCrouchState);
                    }
                } else if (player.playerInputManager.WalkInput) player.psm.ChangeState(player.psm.walkingState);
                else if (player.playerInputManager.SprintInput && player.playerInputManager.SprintInputTimer >= 0.3f)
                    player.psm.ChangeState(player.psm.sprintingState);
            }
        }

        protected override void HandleRotation() {
            base.HandleRotation();
            if (lookingDirection == Vector3.zero) {
                lookingDirection = player.transform.forward;
            }
            Quaternion tr = Quaternion.LookRotation(lookingDirection);
            Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
            player.transform.rotation = targetRotation;
        }

        protected override void HandleMovement() {
            //Debug.Log("Move Direction Before Call Base : " + moveDirection);
            base.HandleMovement();
            //Debug.Log("Move Direction After Call Base : " + moveDirection);
            float speed = player.moveSpeed * moveSpeedModifier;
            moveDirection *= speed;
            //Debug.Log(moveDirection);

            //if (moveDirection.magnitude > CharacterMaximumVelocity.magnitude)
            //    CharacterMaximumVelocity = moveDirection;
            //Debug.Log(moveDirection.magnitude);
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}