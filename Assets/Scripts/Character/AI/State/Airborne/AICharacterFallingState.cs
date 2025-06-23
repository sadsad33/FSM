using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterFallingState : AICharacterAirborneState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.aiAnimatorManager.PlayAnimation("Falling", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (aiCharacter.InAirTimer != 0)
                inAirTimer = aiCharacter.InAirTimer;
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            inAirTimer = 0f;
        }

        public override void Thinking() {
            base.Thinking();
            if(aiCharacter.isGrounded) {
                //Debug.Log("ÂøÁö ¼Óµµ : " + MovingVelocityInAir);
                //if (inAirTimer <= 0.5f) {
                //    if (moveAmount <= 0f) {
                //        //Debug.Log("aiCharacter InAirTimer : " + aiCharacter.InAirTimer);
                //        aiCharacter.acsm.ChangeState(aiCharacter.acsm.lightLandingState);
                //    } else {
                //        //Debug.Log("aiCharacter InAirTimer : " + aiCharacter.InAirTimer);
                //        aiCharacter.acsm.ChangeState(aiCharacter.acsm.landToMoveState);
                //    }
                //} else if (inAirTimer <= 1f) {
                //    //Debug.Log("aiCharacter InAirTimer : " + aiCharacter.InAirTimer);
                //    aiCharacter.acsm.ChangeState(aiCharacter.acsm.mediumLandingState);
                //} else {
                //    //Debug.Log("aiCharacter InAirTimer : " + aiCharacter.InAirTimer);
                //    aiCharacter.acsm.ChangeState(aiCharacter.acsm.hardLandingState);
                //}
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
                MovingVelocityInAir = Vector3.zero;
            }
        }
    }
}