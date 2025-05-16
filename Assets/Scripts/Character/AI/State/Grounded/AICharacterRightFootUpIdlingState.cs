using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KBH {
    public class AICharacterRightFootUpIdlingState : AICharacterGroundedState {

        // 현재 링크 의 Start와 End 사이에서 Start -> End 방향으로 움직일건지 End -> Start 방향으로 움직일건지, 즉 윗방향으로 사다리를 올라갈건지 아래방향으로 사다리를 내려갈건지를 구해야 할듯
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