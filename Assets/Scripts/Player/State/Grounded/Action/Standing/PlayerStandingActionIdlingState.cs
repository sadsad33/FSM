using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingActionIdlingState : PlayerGroundedActionIdlingState
{
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
        else if (player.playerInputManager.LightAttackInput) {
            player.pasm.ChangeState(player.pasm.oneHandSwordFirstAttackState);
        } else if (player.playerInputManager.HeavyAttackInput) {
            player.pasm.ChangeState(player.pasm.oneHandSwordHeavyAttackState);
        }
    }
}
