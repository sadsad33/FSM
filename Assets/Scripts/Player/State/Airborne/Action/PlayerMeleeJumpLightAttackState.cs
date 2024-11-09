using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeJumpLightAttackState : PlayerAirborneActionIdlingState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isAttacking = true;
        player.playerAnimatorManager.PlayAnimation("Melee Jump Light Attack", false);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {

    }

    public override void HandleInput() {
        base.HandleInput();
        if (player.isGrounded)
            player.pasm.ChangeState(player.pasm.lightAttackLandingState);
    }
}
