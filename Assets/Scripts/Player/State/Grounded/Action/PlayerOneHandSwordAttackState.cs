using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHandSwordAttackState : PlayerActionState
{
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
    }

    public override void HandleInput() {
        base.HandleInput();
    }
}
