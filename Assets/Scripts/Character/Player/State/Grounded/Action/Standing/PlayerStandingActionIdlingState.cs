using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KBH {
    public class PlayerStandingActionIdlingState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.canAttackDuringAction = false;
            player.playerAnimatorManager.PlayAnimation("One Hand Idle", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.playerStatsManager.currentStamina <= 0f) return;
            if (player.isPerformingAction) return;
            else if (player.playerInputManager.LightAttackInput) {
                if (!TryFatalBlow())
                    player.pasm.ChangeState(player.pasm.oneHandSwordFirstAttackState);
            } else if (player.playerInputManager.HeavyAttackInput) {
                player.pasm.ChangeState(player.pasm.oneHandSwordHeavyAttackState);
            } else if (player.playerInputManager.WeaponArtInput) {
                player.pasm.ChangeState(player.pasm.weaponArtActionState);
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
                player.pasm.ChangeState(player.pasm.ripostingState);
                return true;
            } else if (Physics.Raycast(ray, out hit, 0.5f, player.playerInteractionManager.GetBackstabLayerMask())) {
                enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                if (enemyCharacter != null) {
                    enemyCharacter.beingBackstabbed = true;
                    player.transform.position = enemyCharacter.characterInteractionManager.GetBackstabbingCharacterStandingPosition().position;
                    player.transform.forward = enemyCharacter.transform.forward;
                }
                player.pasm.ChangeState(player.pasm.backstabbingState);
                return true;
            }
            return false;
        }
    }
}
