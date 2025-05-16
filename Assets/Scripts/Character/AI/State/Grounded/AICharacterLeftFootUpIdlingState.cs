using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLeftFootUpIdlingState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            int index = -1;
            if (aiCharacter.agent.isOnOffMeshLink)
                index = GetNextCornerIndex();

            Vector3 nextCorner;
            if (index >= 0 && index + 1 < aiCharacter.agent.path.corners.Length) {
                nextCorner = aiCharacter.agent.path.corners[index + 1];
                Debug.Log("Next Corner : " + nextCorner);
                //Debug.Log("현재 AI의 위치 : " + aiCharacter.transform.position);
                //Debug.Log("다음 코너의 위치 ; " + nextCorner);
                if (nextCorner.y - aiCharacter.transform.position.y >= 0.1f) {
                    if (aiCharacter.isOnLadderTopEdge) {
                        aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLadderTopEndInteractionState);
                    } else aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiClimbingUpState);
                } else {
                    if (aiCharacter.isOnLadderBottomEdge) {
                        aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLadderBottomEndInteractionState);
                    } else aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiClimbingDownState);
                }

            }
        }

        private int GetNextCornerIndex() {
            int ret = 0;
            while (true) {
                if (aiCharacter.agent.path.corners[ret] == aiCharacter.agent.currentOffMeshLinkData.startPos ||
                    aiCharacter.agent.path.corners[ret] == aiCharacter.agent.currentOffMeshLinkData.endPos) {
                    break;
                }
                ret++;
            }
            return ret;
        }

    }
}