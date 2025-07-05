using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerGroundedState : PlayerMovementState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.InAirTimer = 0f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //Debug.Log("Grounded State Stay ÀÇ MoveDirection : " + moveDirection);
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            //Debug.Log("Grounded State HandleInput ÀÇ MoveDirection : " + moveDirection);
            if (!player.isGrounded) {
                //if (player.InAirTimer >= 0.2f) {
                player.psm.ChangeState(player.psm.fallingState);
                //player.pasm.ChangeState(player.pasm.airborneActionIdlingState);
                //}
            } else {
                if (player.playerInputManager.RollFlag) {
                    if (player.playerInputManager.SprintInputTimer > 0f && player.playerInputManager.SprintInputTimer < 0.3f) {
                        if (player.playerStatsManager.currentStamina > 0f && !player.isPerformingAction)
                            player.psm.ChangeState(player.psm.rollingState);
                    }
                }
                if (player.playerInputManager.LightAttackInput) {
                    if (player.isPerformingAction || player.playerStatsManager.currentStamina <= 10f) return;
                    else {
                        if (player.pasm.GetCurrentState() == player.pasm.standingActionIdlingState) {
                            if (!TryFatalBlow())
                                player.psm.ChangeState(player.psm.oneHandWeaponLightAttackState);
                        } else if (player.pasm.GetCurrentState() == player.pasm.crouchedActionIdlingState) {
                            MeleeWeaponItem item = player.playerEquipmentManager.rightHandSlot.GetItemOnSlot() as MeleeWeaponItem;
                            if (item.crouchAttackAnimation == "") return;
                            player.psm.ChangeState(player.psm.crouchingAttackState);
                        }
                    }
                } else if (player.playerInputManager.HeavyAttackInput) {
                    if (player.isPerformingAction || player.playerStatsManager.currentStamina <= 20f) return;
                    else if (player.pasm.GetCurrentState() == player.pasm.standingActionIdlingState) {
                        player.psm.ChangeState(player.psm.oneHandWeaponHeavyAttackState);
                    }
                } else if (player.playerInputManager.WeaponArtInput) {
                    if (player.isPerformingAction) return;
                    else if (player.pasm.GetCurrentState() == player.pasm.standingActionIdlingState) {
                        player.psm.ChangeState(player.psm.weaponArtActionState);
                    }
                }
            }
        }

        public bool TryFatalBlow() {
            Debug.DrawLine(player.playerInteractionManager.GetFatalBlowRaycastStartingPosition(), player.transform.forward, Color.red);
            Ray ray = new(player.playerInteractionManager.GetFatalBlowRaycastStartingPosition(), player.transform.forward);
            RaycastHit hit;
            CharacterManager enemyCharacter;
            if (Physics.Raycast(ray, out hit, 1.5f, player.playerInteractionManager.GetRiposteLayerMask())) {
                enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                if (enemyCharacter != null) {
                    enemyCharacter.beingRiposted = true;
                    player.transform.position = enemyCharacter.characterInteractionManager.GetRipostingCharacterStandingPosition().position;
                    player.transform.forward = -enemyCharacter.transform.forward;
                }
                player.psm.ChangeState(player.psm.ripostingState);
                return true;
            } else if (Physics.Raycast(ray, out hit, 0.5f, player.playerInteractionManager.GetBackstabLayerMask())) {
                enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                if (enemyCharacter != null) {
                    enemyCharacter.beingBackstabbed = true;
                    player.transform.position = enemyCharacter.characterInteractionManager.GetBackstabbingCharacterStandingPosition().position;
                    player.transform.forward = enemyCharacter.transform.forward;
                }
                player.psm.ChangeState(player.psm.backstabbingState);
                return true;
            }
            return false;
        }
    }
}