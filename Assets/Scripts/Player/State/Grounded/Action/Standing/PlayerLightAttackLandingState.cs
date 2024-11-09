using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttackLandingState : PlayerActionState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("Light Attack Landing", player.isPerformingAction);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        base.Exit(character);
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isAttacking)
            player.pasm.ChangeState(player.pasm.standingActionIdlingState);
    }       
}
