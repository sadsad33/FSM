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
                if (inAirTimer <= 0.5f) {
                    if (moveAmount <= 0f) {
                        //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                        player.pmsm.ChangeState(player.pmsm.lightLandingState);
                    }
                    else {
                        //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                        player.pmsm.ChangeState(player.pmsm.landToMoveState);
                    }
                }
                else if (inAirTimer <= 1f) {
                    //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                    player.pmsm.ChangeState(player.pmsm.mediumLandingState);
                }
                else {
                    //Debug.Log("Player InAirTimer : " + player.InAirTimer);
                    player.pmsm.ChangeState(player.pmsm.hardLandingState);
                }
                MovingVelocityInAir = Vector3.zero;
            }
        }
    }
}