using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerCrouchToStandState : PlayerGroundedState {
        public Vector3 tr;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerAnimatorManager.disableOnAnimatorMove = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Crouched To Standing", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //if (player.cc.enabled)
            //    player.cc.Move(Vector3.zero);
            if (tr != null) {
                //Debug.Log("Crouch To Stand : " + tr);
                player.transform.position = tr;
            }
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            player.isCrouched = false;
            player.transform.position = tr;
            player.playerAnimatorManager.disableOnAnimatorMove = false;
            player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }

        public override void HandleInput() {
            base.HandleInput();
            //if (!player.isPerformingAction)
            player.pmsm.ChangeState(player.pmsm.idlingState);
        }
    }
}