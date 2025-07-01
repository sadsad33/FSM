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
            // �̵� Ű + Left Shift => Sprint
            // ���� ���¿��� Left Shift �� �̵� Ű ���� �ϳ��� �Է��� ���ŵȴٸ� Medium Stopping State�� ����
            // �̶� �̵�Ű �Է��� ���� �����ϸ�, Sprinting State�� ����Ǹ鼭 SprintInputTimer �� 0���� �ʱ�ȭ������, ���� Left Shift �Է��� true �̹Ƿ� SprintInputTimer �� �ٽ� ���ư�
            // Left Shift ���� �ڴʰ� ���� ���� ���� ���, InputTimer �� �ð��� 0.3 �� �̸��̶�� RollFlag �� true �� �Ǿ� Rolling State�� ���̵�
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.playerStatsManager.currentStamina <= 0) player.psm.ChangeState(player.psm.runningState);
            if (moveAmount <= 0f) {
                player.psm.ChangeState(player.psm.mediumStoppingState);
            } else {
                if (player.playerInputManager.LightAttackInput) {
                    if (player.playerStatsManager.currentStamina <= 10f) return;
                    player.psm.ChangeState(player.psm.runningAttackState);
                } else if (player.playerInputManager.JumpInput) {
                    player.psm.runningJumpState.MovingVelocityInAir = moveDirection;
                    player.psm.ChangeState(player.psm.runningJumpState);
                } else if (!player.playerInputManager.SprintInput) {
                    player.psm.ChangeState(player.psm.runningState);
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
            //Debug.Log(moveDirection.magnitude);
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}