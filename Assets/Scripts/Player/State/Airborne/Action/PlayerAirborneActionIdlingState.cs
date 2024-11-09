using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneActionIdlingState : PlayerActionState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerAnimatorManager.PlayAnimation("One Hand Idle", false);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (player.canAttackDuringAction && player.playerInputManager.LightAttackInput)
            player.pasm.ChangeState(player.pasm.meleeJumpLightAttackState);
    }
}
