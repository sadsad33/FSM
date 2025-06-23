using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KBH {
    public class AICharacterPursuingState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!aiCharacter.cc.enabled) aiCharacter.cc.enabled = true;
            aiCharacter.agent.enabled = true;
            aiCharacter.agent.updateRotation = true;
            moveSpeedModifier = 2f;
            //Debug.Log(aiCharacter.isGrounded);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            //aiCharacter.agent.enabled = false;
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0f);
        }

        public override void Thinking() {
            HandleMovement();
            if (aiCharacter.agent.isOnOffMeshLink) {
                aiCharacter.agent.autoTraverseOffMeshLink = false;
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiMoveToInteractingPositionState);
            } else if (Vector3.Distance(aiCharacter.aiEyesManager.currentTarget.transform.position, aiCharacter.transform.position) <= aiCharacter.aiStatsManager.CombatStanceDistance) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            }
        }

        private void HandleMovement() {
            //Debug.Log("움직이기");
            aiCharacter.agent.SetDestination(aiCharacter.aiEyesManager.currentTarget.transform.position);
            moveDirection = aiCharacter.agent.desiredVelocity;
            if (aiCharacter.cc.enabled)
                aiCharacter.cc.Move(Time.deltaTime * moveDirection);
        }
    }
}
