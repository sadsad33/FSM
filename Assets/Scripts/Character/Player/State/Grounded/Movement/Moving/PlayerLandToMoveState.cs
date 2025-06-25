using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLandToMoveState : PlayerLandingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking && !player.isPerformingAction) {
                player.isMoving = true;
                player.playerAnimatorManager.PlayAnimation("Land To Move", player.isMoving);
            }
        }

        public override void Stay(CharacterManager character) {
            //Debug.Log(player.isPerformingAction);
            base.Stay(character);
            //player.cc.Move(Vector3.zero);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isMoving) {
                player.pmsm.ChangeState(player.pmsm.idlingState);
            }
        }
    }
}