using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLadderTopStartInteractionState : AICharacterGroundedInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            curInteractable = aiInteraction.currentInteractable;
            aiCharacter.isClimbing = true;
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
                //Debug.Log(curInteractable.GetComponent<Ladder>().isTop);
                Quaternion tr = Quaternion.LookRotation(-curInteractable.transform.right);
                Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, tr, 5 * Time.deltaTime);
                aiCharacter.transform.SetPositionAndRotation(Vector3.Slerp(aiCharacter.transform.position, targetPosition, 12 * Time.deltaTime), targetRotation);
            }
        }

        public override void Exit(CharacterManager character) {
            aiCharacter.aiInteractionManager.isInteracting = false;
            aiCharacter.aiAnimatorManager.disableOnAnimatorMove = false;
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction && aiCharacter.transform.position == targetPosition) {
                aiCharacter.cc.enabled = true;
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiRightFootUpIdlingState);
            }
        }
    }
}