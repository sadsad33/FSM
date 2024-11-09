using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideAttackState : PlayerActionState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isAttacking = true;
        player.playerAnimatorManager.PlayAnimation("Slide Attack", player.isAttacking);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        base.Exit(character);
        player.isMoving = false;
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isAttacking)
            player.pasm.ChangeState(player.pasm.standingActionIdlingState);
    }
}
