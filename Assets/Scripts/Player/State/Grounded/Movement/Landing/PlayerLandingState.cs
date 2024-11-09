using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerGroundedState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        //player.InAirTimer = 0f;
        player.canSliding = false;
        if (!player.isAttacking)
            player.isPerformingAction = true;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
    }
}
