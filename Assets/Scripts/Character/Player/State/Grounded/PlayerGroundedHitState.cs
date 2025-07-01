using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerGroundedHitState : PlayerGroundedState {

        string damageAnimation;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isPerformingAction = true;
            if (player.isJumping) player.isJumping = false;
            if (player.isCrouching) player.isCrouching = false;
            player.playerAnimatorManager.animator.applyRootMotion = player.isPerformingAction;
            player.playerAnimatorManager.animator.CrossFade(damageAnimation, 0f, 3, 0f);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.psm.ChangeState(player.psm.idlingState);
            }
        }

        public void SetDamageAnimation(string damageAnimation) => this.damageAnimation = damageAnimation;
    }
}