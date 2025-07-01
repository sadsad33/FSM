using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerFallingState : PlayerAirborneState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking && !player.isPerformingAction) {
                player.playerAnimatorManager.PlayAnimation("Falling", false);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (player.InAirTimer != 0)
                inAirTimer = player.InAirTimer;
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            inAirTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.isGrounded) {
                //Debug.Log("ÂøÁö ¼Óµµ : " + MovingVelocityInAir);
                if (player.isAttacking) {
                    player.psm.ChangeState(player.psm.lightAttackLandingState);
                } else if (inAirTimer <= 0.5f) {
                    if (moveAmount <= 0f) {
                        //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                        player.psm.ChangeState(player.psm.lightLandingState);
                    } else {
                        //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                        player.psm.ChangeState(player.psm.landToMoveState);
                    }
                } else if (inAirTimer <= 1f) {
                    //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                    player.psm.ChangeState(player.psm.mediumLandingState);
                } else {
                    //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                    player.psm.ChangeState(player.psm.hardLandingState);
                }
                MovingVelocityInAir = Vector3.zero;
            }
        }
    }
}