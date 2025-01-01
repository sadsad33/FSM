using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRunningJumpState : PlayerAirborneState {
        private float maximumHeight;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.pasm.ChangeState(player.pasm.airborneActionIdlingState);
            player.playerStatsManager.DeductStamina(25f);
            player.consumingStamina = true;
            Vector3 playerYVelocity;
            player.isJumping = true;
            maximumHeight = player.transform.position.y + player.MaximumJumpHeight;
            playerYVelocity = player.YVelocity;
            playerYVelocity.y = player.JumpStartYVelocity;
            player.YVelocity = playerYVelocity;
            player.playerAnimatorManager.PlayAnimation("Running Jump", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //Debug.Log("Running Jump State Stay ÀÇ MoveDirection : " + moveDirection);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
            HandleJumping();
        }

        public override void Exit(CharacterManager character) {
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.transform.position.y >= maximumHeight) {
                player.isJumping = false;
                player.pmsm.fallingState.MovingVelocityInAir = (MovingVelocityInAir + aeroInputDirection).normalized;
                player.pmsm.ChangeState(player.pmsm.fallingState);
            }
        }

        private void HandleJumping() {
            if (player.transform.position.y < maximumHeight) {
                Vector3 tempVelocity = player.YVelocity;
                tempVelocity.y += player.JumpForce * Time.deltaTime;
                player.YVelocity = tempVelocity;
                if (player.cc.enabled)
                    player.cc.Move(player.YVelocity * Time.deltaTime);
            }
        }
    }
}