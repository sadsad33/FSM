using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLightLandingState : PlayerLandingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking)
                player.playerAnimatorManager.PlayAnimation("Light Landing", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //if (moveAmount > 0) player.isPerformingAction = false;
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            //if (!player.isPerformingAction) {
            player.pmsm.ChangeState(player.pmsm.idlingState);
            //}
        }
    }
}