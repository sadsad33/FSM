using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStandingJumpState : PlayerAirborneState {
        private float maximumHeight;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(20f);
            player.consumingStamina = true;
            player.pasm.ChangeState(player.pasm.airborneActionIdlingState);
            Vector3 playerYVelocity;
            player.isJumping = true;
            maximumHeight = player.transform.position.y + player.MaximumJumpHeight;
            playerYVelocity = player.YVelocity;
            playerYVelocity.y = player.JumpStartYVelocity;
            player.YVelocity = playerYVelocity;
            player.playerAnimatorManager.PlayAnimation("Standing Jump", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
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