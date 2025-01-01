using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRunningState : PlayerMovingState {

        private float canSlidingDelayTimer;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
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
                    player.pmsm.ChangeState(player.pmsm.lightStoppingState);
                else
                    player.pmsm.ChangeState(player.pmsm.idlingState);
            } else if(!player.isPerformingAction){
                if (player.playerInputManager.JumpInput && player.playerStatsManager.currentStamina > 10f) {
                    player.pmsm.runningJumpState.MovingVelocityInAir = moveDirection;
                    player.pmsm.ChangeState(player.pmsm.runningJumpState);
                } else if (player.playerInputManager.CrouchInput) {
                    if (player.canSliding)
                        player.pmsm.ChangeState(player.pmsm.slidingState);
                    else if (!player.isCrouched) {
                        player.pmsm.standToCrouchState.tr = player.transform.position;
                        player.pmsm.ChangeState(player.pmsm.standToCrouchState);
                    }
                } else if (player.playerInputManager.WalkInput) player.pmsm.ChangeState(player.pmsm.walkingState);
                else if (player.playerInputManager.SprintInput && player.playerInputManager.SprintInputTimer >= 0.3f)
                    player.pmsm.ChangeState(player.pmsm.sprintingState);
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
            if (moveDirection.magnitude > PlayerMaximumVelocity.magnitude)
                PlayerMaximumVelocity = moveDirection;
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}