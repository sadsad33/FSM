using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerSlideAttackState : PlayerActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //player.pmsm.ChangeState(player.pmsm.notMovingState);
            player.isAttacking = true;
            player.playerAnimatorManager.PlayAnimation("Slide Attack", player.isAttacking);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            //player.pmsm.ChangeState(player.pmsm.idlingState);
            player.isMoving = false;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isAttacking)
                player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }
    }
}