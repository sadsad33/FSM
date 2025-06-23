using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLadderTopStartInteractionState : AICharacterGroundedInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            curInteractable = aiInteraction.currentInteractable;
            aiCharacter.isMoving = true;
            aiCharacter.isClimbing = true;
            aiCharacter.cc.enabled = false;
            aiCharacter.isPerformingAction = true;
            aiCharacter.agent.updateRotation = false;
            aiCharacter.aiInteractionManager.isInteracting = true;
            aiCharacter.aiAnimatorManager.disableOnAnimatorMove = true;

            aiCharacter.aiAnimatorManager.PlayAnimation("Ladder_StartTop", aiCharacter.isPerformingAction);
            if (curInteractable != null) {
                targetPosition = curInteractable.GetComponent<Ladder>().GetClimbingStartPosition();
                curInteractable.Interact(aiCharacter);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (curInteractable != null) {
                Quaternion tr = Quaternion.LookRotation(-curInteractable.transform.right);
                Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, tr, 3 * Time.deltaTime);
                aiCharacter.transform.SetPositionAndRotation(Vector3.Slerp(aiCharacter.transform.position, targetPosition, 1.5f * Time.deltaTime), targetRotation);
                //aiCharacter.transform.rotation *= aiCharacter.aiAnimatorManager.animator.deltaRotation;
                //aiCharacter.transform.position += aiCharacter.aiAnimatorManager.animator.deltaPosition;
            }
        }

        public override void Exit(CharacterManager character) {
            aiCharacter.isMoving = false;
            aiCharacter.aiInteractionManager.isInteracting = false;
            aiCharacter.aiAnimatorManager.disableOnAnimatorMove = false;
        }

        public override void Thinking() {
            base.Thinking();
            //float dist = Vector3.Distance(aiCharacter.transform.position, targetPosition);
            if (!aiCharacter.isPerformingAction) {
                aiCharacter.cc.enabled = true;
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiRightFootUpIdlingState);
            }
        }
    }
}