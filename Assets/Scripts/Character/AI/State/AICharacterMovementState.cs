using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterMovementState : CharacterMovementState {
        protected AICharacterManager aiCharacter;
        protected AICharacterEyesManager aiCharacterEyes;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //Debug.Log("AI Current State : " + this);
            aiCharacter = character as AICharacterManager;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //Debug.Log("Handling Gravity");
            Thinking();
        }

        public override void Exit(CharacterManager character) {
            
        }

        public virtual void Thinking() {
            HandleRotation();
        }

        void HandleRotation() {
            if (aiCharacter.isClimbing) return;
            if (aiCharacter.aiEyesManager.currentTarget != null) {
                if (!aiCharacter.isPerformingAction || aiCharacter.canRotateDuringAction) {
                    //Debug.Log("È¸Àü");
                    Vector3 lookDirection = aiCharacter.aiEyesManager.currentTarget.transform.position - aiCharacter.transform.position;
                    lookDirection.y = 0;
                    lookDirection.Normalize();

                    if (lookDirection == Vector3.zero) {
                        lookDirection = aiCharacter.transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(lookDirection);
                    aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, tr, 5 * Time.deltaTime);
                }
            }
        }
    }
}
