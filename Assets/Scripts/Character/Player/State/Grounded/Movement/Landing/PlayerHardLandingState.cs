using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerHardLandingState : PlayerLandingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking && !player.isPerformingAction) {
                player.isPerformingAction = true;
                player.playerAnimatorManager.PlayAnimation("Hard Landing", player.isPerformingAction);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction)
                player.psm.ChangeState(player.psm.idlingState);
        }
    }
}