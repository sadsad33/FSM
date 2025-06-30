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
            if (player.playerAnimatorManager.animator.GetCurrentAnimatorStateInfo(3).IsName(damageAnimation)) {
                //Debug.Log("애니메이션 강제 재생");
                player.playerAnimatorManager.animator.applyRootMotion = player.isPerformingAction;
                player.playerAnimatorManager.animator.Play(damageAnimation, 3, 0f);
                player.playerAnimatorManager.animator.Update(0);
            } else {
                //Debug.Log("애니메이션 재생");
                player.playerAnimatorManager.PlayAnimation(damageAnimation, player.isPerformingAction);
            }
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
                player.pmsm.ChangeState(player.pmsm.idlingState);
            }
        }

        public void SetDamageAnimation(string damageAnimation) => this.damageAnimation = damageAnimation;
    }
}