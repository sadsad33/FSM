using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerCrouchingAttackState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isPerformingAction = true;
            //player.pmsm.ChangeState(player.pmsm.notMovingState);
            player.playerAnimatorManager.PlayAnimation("Crouching Attack", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            //player.pmsm.ChangeState(player.pmsm.idlingState);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction)
                //player.pasm.ChangeState(player.pasm.crouchedActionIdlingState);
                player.psm.ChangeState(player.psm.crouchedIdlingState);
        }
    }
}