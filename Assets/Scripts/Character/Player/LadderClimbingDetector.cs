using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    // 플레이어가 사다리를 타는 동안에만 이 스크립트가 Enable 되어야 할듯
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
