using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHandSwordFirstAttackState : PlayerActionState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("OH_Sword_Attack1", player.isPerformingAction);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        base.Exit(character);
        player.isPerformingAction = false;
    }

    public override void HandleInput() {
        base.HandleInput();
        //if (!player.isPerformingAction) 
        player.pasm.ChangeState(player.pasm.actionIdlingState);
    }
}
