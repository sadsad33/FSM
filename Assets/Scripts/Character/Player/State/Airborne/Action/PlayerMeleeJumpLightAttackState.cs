using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerMeleeJumpLightAttackState : PlayerAirborneActionIdlingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(15f);
            player.consumingStamina = true;
            player.isAttacking = true;
            player.playerAnimatorManager.PlayAnimation("Melee Jump Light Attack", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.isGrounded)
                player.pasm.ChangeState(player.pasm.lightAttackLandingState);
        }
    }
}