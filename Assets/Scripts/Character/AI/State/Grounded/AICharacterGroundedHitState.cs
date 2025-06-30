using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterGroundedHitState : AICharacterGroundedState {

        string damageAnimation;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            if (aiCharacter.isJumping) aiCharacter.isJumping = false;
            if (aiCharacter.isCrouching) aiCharacter.isCrouching = false;
            
            if (aiCharacter.aiAnimatorManager.animator.GetCurrentAnimatorStateInfo(3).IsName(damageAnimation)) {
                //Debug.Log("애니메이션 강제 재생");
                aiCharacter.aiAnimatorManager.animator.applyRootMotion = aiCharacter.isPerformingAction;
                aiCharacter.aiAnimatorManager.animator.Play(damageAnimation, 3, 0f);
                aiCharacter.aiAnimatorManager.animator.Update(0);
            } else {
                //Debug.Log("애니메이션 재생");
                aiCharacter.aiAnimatorManager.PlayAnimation(damageAnimation, aiCharacter.isPerformingAction);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction)
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
        }

        public void SetDamageAnimation(string damageAnimation) => this.damageAnimation = damageAnimation;
    }
}
