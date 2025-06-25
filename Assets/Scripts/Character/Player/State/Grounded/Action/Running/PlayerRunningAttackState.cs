using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRunningAttackState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //player.pmsm.ChangeState(player.pmsm.idlingState);
            player.isAttacking = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Running Attack", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.isAttacking = false;
            player.isPerformingAction = false;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction)
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }
    }
}