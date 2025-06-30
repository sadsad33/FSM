using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLightAttackLandingState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(15f);
            player.consumingStamina = true;
            player.isAttacking = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Light Attack Landing", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isAttacking && !player.isPerformingAction)
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }
    }
}