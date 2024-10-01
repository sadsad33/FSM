using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerAnimatorManager.PlayAnimation("Falling", false);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (player.InAirTimer <= 0.3f) player.pmsm.ChangeState(player.pmsm.lightLandingState);
        else if (player.InAirTimer <= 0.6f) player.pmsm.ChangeState(player.pmsm.mediumLandingState);
        else if (player.InAirTimer <= 1f) player.pmsm.ChangeState(player.pmsm.hardLandingState);
    }
}
