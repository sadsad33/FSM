using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    // 플레이어가 사다리를 타는 동안에만 이 스크립트가 Enable 되어야 할듯
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
