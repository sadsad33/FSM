using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterMoveToInteractingPositionState : AICharacterGroundedInteractionState {
        public Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.cc.enabled = false;
            aiCharacter.aiAnimatorManager.disableOnAnimatorMove = true;
            curInteractable = aiInteraction.currentInteractable;
            aiCharacter.isMoving = true;
            aiCharacter.isPerformingAction = true;
            if (curInteractable != null)
                targetPosition = curInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
            if (curInteractable != null) {
                aiCharacter.transform.position = Vector3.MoveTowards(aiCharacter.transform.position, targetPosition, 2 * Time.deltaTime);
                Vector3 lookDir = curInteractable.transform.right;
                lookDir.y = 0;
                if (lookDir != Vector3.zero) {
                    Quaternion tr = Quaternion.LookRotation(lookDir);
                    Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, tr, 5 * Time.deltaTime);
                    aiCharacter.transform.rotation = targetRotation;
                }
            }
        }

        public override void Exit(CharacterManager character) {
            aiCharacter.cc.enabled = true;
            aiCharacter.aiAnimatorManager.disableOnAnimatorMove = false;
            aiCharacter.isMoving = false;
            aiCharacter.isPerformingAction = false;
        }

        public override void Thinking() {
            base.Thinking();
            if (aiCharacter.transform.position == targetPosition) {
                aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0f);
                Ladder currentLadder = curInteractable.GetComponent<Ladder>();
                if (currentLadder != null) {
                    if (currentLadder.isTop) {
                        aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLadderTopStartInteractionState);
                    } else {
                        aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLadderBottomStartInteractionState);
                    }
                }
            }
        }
    }
}