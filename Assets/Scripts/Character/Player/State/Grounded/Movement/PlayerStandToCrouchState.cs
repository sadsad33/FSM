using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStandToCrouchState : PlayerMovingState {
        public Vector3 tr;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //Debug.Log(player.isCrouched);
            player.isCrouching = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.disableOnAnimatorMove = true;
            player.playerAnimatorManager.PlayAnimation("Standing To Crouched", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            player.cc.enabled = true;
            player.playerAnimatorManager.disableOnAnimatorMove = false;
            //player.pasm.ChangeState(player.pasm.crouchedActionIdlingState);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction)
                player.psm.ChangeState(player.psm.crouchedIdlingState);
        }
    }
}