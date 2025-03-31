using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLandToMoveState : PlayerLandingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //Debug.Log(player.isPerformingAction);
            if (!player.isAttacking)
                player.playerAnimatorManager.PlayAnimation("Land To Move", player.isPerformingAction);
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
            //if (!player.isPerformingAction) {
            if (player.playerInputManager.SprintInput)
                player.pmsm.ChangeState(player.pmsm.sprintingState);
            else if (player.playerInputManager.WalkInput)
                player.pmsm.ChangeState(player.pmsm.walkingState);
            else
                player.pmsm.ChangeState(player.pmsm.runningState);
            //}
        }
    }
}