using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerAirborneHitState : PlayerAirborneState {

        float inAirTimer = 0f;

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isPerformingAction) player.isPerformingAction = true;
            if (player.isJumping) player.isJumping = false;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            inAirTimer += Time.deltaTime;
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isGrounded) {
                if (inAirTimer >= 1f) {
                    // 등을 바닥으로 향하고 떨어지는 상태로 전환
                }
            } else {
                // 바닥에 자빠지는 상태로 전환
            }
        }
    }
}