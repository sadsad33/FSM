using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerAirborneActionIdlingState : PlayerAirborneActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerAnimatorManager.PlayAnimation("One Hand Idle", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            //if (player.playerStatsManager.currentStamina <= 10f) return;
            //else if (player.canAttackDuringAction && player.playerInputManager.LightAttackInput) {
            //    player.pasm.ChangeState(player.pasm.meleeJumpLightAttackState);
            //}
        }
    }
}