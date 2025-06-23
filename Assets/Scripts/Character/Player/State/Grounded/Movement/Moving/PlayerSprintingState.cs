using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerSprintingState : PlayerMovingState {

        float accelerateTimer;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.consumingStamina = true;
            player.pasm.ChangeState(player.pasm.sprintingActionIdlingState);
            moveSpeedModifier = 1.6f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerStatsManager.DeductStamina(0.1f);
            accelerateTimer += Time.deltaTime;
            if (accelerateTimer >= 0.5f && player.playerStatsManager.currentStamina > 10f) {
                player.canSliding = true;
            }
            player.RunningStateTimer += Time.deltaTime;
            //HandleMovement();
            player.playerAnimatorManager.animator.SetFloat("Vertical", 2.0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            // 이동 키 + Left Shift => Sprint
            // 질주 상태에서 Left Shift 나 이동 키 둘중 하나의 입력이 제거된다면 Medium Stopping State로 전이
            // 이때 이동키 입력을 먼저 제거하면, Sprinting State가 종료되면서 SprintInputTimer 가 0으로 초기화되지만, 아직 Left Shift 입력은 true 이므로 SprintInputTimer 는 다시 돌아감
            // Left Shift 에서 뒤늦게 손을 떼게 됐을 경우, InputTimer 의 시간이 0.3 초 미만이라면 RollFlag 가 true 가 되어 Rolling State로 전이됨
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.playerStatsManager.currentStamina <= 0) player.pmsm.ChangeState(player.pmsm.runningState);
            if (moveAmount <= 0f) {
                player.pmsm.ChangeState(player.pmsm.mediumStoppingState);
            } else {
                if (player.playerInputManager.JumpInput) {
                    player.pmsm.runningJumpState.MovingVelocityInAir = moveDirection;
                    player.pmsm.ChangeState(player.pmsm.runningJumpState);
                } else if (!player.playerInputManager.SprintInput) {
                    player.pmsm.ChangeState(player.pmsm.runningState);
                }
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