using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerCrouchedIdlingState : PlayerGroundedState {
        public Vector3 tr;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.pasm.ChangeState(player.pasm.crouchedActionIdlingState);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.playerInputManager.CrouchInput) {
                player.psm.ChangeState(player.psm.crouchToStandState);
            } else if (moveAmount > 0f)
                player.psm.ChangeState(player.psm.crouchedWalkingState);
        }
    }
}