using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    // �÷��̾ ��ٸ��� Ÿ�� ���ȿ��� �� ��ũ��Ʈ�� Enable �Ǿ�� �ҵ�
    public class LadderClimbingDetector : MonoBehaviour {
        CharacterManager character;

        private void Awake() {
            character = GetComponentInParent<CharacterManager>();
        }

        private void OnTriggerEnter(Collider other) {
            if (character.isClimbing && other.CompareTag("LadderInteractionEndingPosition")) {
                Ladder ladder = other.transform.parent.GetComponentInChildren<Ladder>();
                if (ladder != null) {
                    if (ladder.isTop) character.isOnLadderTopEdge = true;
                    else character.isOnLadderBottomEdge = true;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("LadderInteractionEndingPosition")) {
                if (character.isOnLadderTopEdge) character.isOnLadderTopEdge = false;
                if (character.isOnLadderBottomEdge) character.isOnLadderBottomEdge = false;
            }
        }
    }
}
