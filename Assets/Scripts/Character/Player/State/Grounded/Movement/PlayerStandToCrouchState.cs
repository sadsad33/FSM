using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStandToCrouchState : PlayerGroundedState {
        public Vector3 tr;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //Debug.Log(player.isCrouched);
            player.isCrouched = true;
            player.isPerformingAction = true;
            //player.playerAnimatorManager.disableOnAnimatorMove = true;
            player.playerAnimatorManager.PlayAnimation("Standing To Crouched", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //if (player.cc.enabled)
            //    player.cc.Move(Vector3.zero);
            if (tr != null) {
                //Debug.Log("Stand To Crouch : " + tr);
                player.transform.position = tr;
            }
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            player.isPerformingAction = false;
            //player.playerAnimatorManager.disableOnAnimatorMove = false;
            player.pasm.ChangeState(player.pasm.crouchedActionIdlingState);
        }

        public override void HandleInput() {
            base.HandleInput();
            //if (!player.isPerformingAction)
            player.pmsm.crouchedIdlingState.tr = tr;
            player.pmsm.ChangeState(player.pmsm.crouchedIdlingState);
        }
    }
}