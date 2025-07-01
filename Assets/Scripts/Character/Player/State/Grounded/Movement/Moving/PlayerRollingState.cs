using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRollingState : PlayerMovingState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(25f);
            player.consumingStamina = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Rolling", player.isPerformingAction);
            player.isInvulnerable = true;
            //player.playerInputManager.RollFlag = false;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            player.isInvulnerable = false;
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.psm.ChangeState(player.psm.idlingState);
            }
        }
    }
}