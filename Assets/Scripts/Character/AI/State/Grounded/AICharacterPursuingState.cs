using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KBH {
    public class AICharacterPursuingState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacterEyes = aiCharacter.aiEyesManager;
            aiCharacter.agent.enabled = true;
            aiCharacter.agent.updateRotation = true;
            moveSpeedModifier = 2f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            //aiCharacter.agent.enabled = false;
        }

        public override void Thinking() {
            HandleMovement();
            if (Vector3.Distance(aiCharacter.aiEyesManager.currentTarget.transform.position, aiCharacter.transform.position) < 1.5f) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            } else if (aiCharacter.agent.isOnOffMeshLink) {
                Debug.Log("Link 도달");
                aiCharacter.agent.autoTraverseOffMeshLink = false;
                //aiCharacter.agent.updatePosition = false;
                //aiCharacter.agent.updateRotation = false;
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiMoveToInteractingPositionState);
            }
        }

        private void HandleMovement() {
            //Debug.Log("움직이기");
            
            aiCharacter.agent.enabled = true;
            aiCharacter.agent.SetDestination(aiCharacterEyes.currentTarget.transform.position);
            moveDirection = aiCharacter.agent.desiredVelocity;
            if (aiCharacter.cc.enabled)
                aiCharacter.cc.Move(Time.deltaTime * moveDirection);
        }
    }
}
