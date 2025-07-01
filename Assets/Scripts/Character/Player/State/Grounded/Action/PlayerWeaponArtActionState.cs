using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerWeaponArtActionState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.isPerformingAction = true;
            // ������ ������ ������� �׸��� �����ؼ� �ִϸ��̼� �̸��� �޾ƿ��� ������
            player.playerAnimatorManager.PlayAnimation("Parry_Parry", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.psm.ChangeState(player.psm.idlingState);
            }
        }
    }
}