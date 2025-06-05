using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLadderBottomEndInteractionState : AICharacterGroundedInteractionState {
        Vector3 targetPosition;

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.cc.enabled = false;
            aiCharacter.isMoving = true;
            aiCharacter.isPerformingAction = true;
            if (aiCharacter.rightFootUp) aiCharacter.aiAnimatorManager.PlayAnimation("Ladder_End_Bottom_RightFootUp", aiCharacter.isPerformingAction);
            else aiCharacter.aiAnimatorManager.PlayAnimation("Ladder_End_Bottom_LeftFootUp", aiCharacter.isPerformingAction);
            targetPosition = aiInteraction.currentInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (Vector3.Distance(aiCharacter.transform.position, targetPosition) > 0.1f) {
                aiCharacter.transform.position = Vector3.Slerp(aiCharacter.transform.position, targetPosition, 2.5f * Time.deltaTime);
            }
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            aiCharacter.cc.enabled = true;
            aiCharacter.isMoving = false;
            aiCharacter.isClimbing = false;
            aiCharacter.agent.autoTraverseOffMeshLink = true;
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction && Vector3.Distance(aiCharacter.transform.position, targetPosition) <= 0.1f) {
                aiCharacter.agent.CompleteOffMeshLink();
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
            }
        }
    }
}
