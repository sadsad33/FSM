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
            //else if (player.playerInputManager.LightAttackInput) {
                //if (!TryFatalBlow())
                    //player.pasm.ChangeState(player.pasm.oneHandSwordFirstAttackState);
            //} else if (player.playerInputManager.HeavyAttackInput) {
            //    player.psm.ChangeState(player.psm.oneHandSwordHeavyAttackState);
            //} else if (player.playerInputManager.WeaponArtInput) {
            //    player.pasm.ChangeState(player.pasm.weaponArtActionState);
            //}
        }

        
    }
}
