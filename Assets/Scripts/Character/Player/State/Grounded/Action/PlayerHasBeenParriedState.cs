using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerHasBeenParriedState : PlayerGroundedActionState{
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Parry_Parried", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
            }
        }
    }
}