using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerClimbingUpState : PlayerClimbingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isPerformingAction = true;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                if (player.rightFootUp)
                    player.pmsm.ChangeState(player.pmsm.rightFootUpIdlingState);
                else
                    player.pmsm.ChangeState(player.pmsm.leftFootUpIdlingState);
            }
        }
    }
}
