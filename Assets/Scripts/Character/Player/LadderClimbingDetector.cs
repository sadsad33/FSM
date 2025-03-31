using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    // �÷��̾ ��ٸ��� Ÿ�� ���ȿ��� �� ��ũ��Ʈ�� Enable �Ǿ�� �ҵ�
    public class LadderClimbingDetector : MonoBehaviour {
        PlayerManager player;

        private void Awake() {
            player = GetComponentInParent<PlayerManager>();
        }

        private void OnTriggerEnter(Collider other) {
            if (player.isClimbing && other.CompareTag("LadderInteractionEndingPosition")) {
                Ladder ladder = other.transform.parent.GetComponentInChildren<Ladder>();
                if (ladder != null) {
                    if (ladder.isTop) player.isOnLadderTopEdge = true;
                    else player.isOnLadderBottomEdge = true;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("LadderInteractionEndingPosition")) {
                if (player.isOnLadderTopEdge) player.isOnLadderTopEdge = false;
                if (player.isOnLadderBottomEdge) player.isOnLadderBottomEdge = false;
            }
        }
    }
}
