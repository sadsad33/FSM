using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionIdlingState : PlayerActionState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
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
        //if (!player.isPerformingAction) {
        if (player.playerInputManager.LightAttackInput) player.pasm.ChangeState(player.pasm.oneHandSwordFirstAttackState);
        //}
    }
}
