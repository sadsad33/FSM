using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumLandingState : PlayerLandingState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerAnimatorManager.PlayAnimation("Medium Landing", true);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        player.pmsm.ChangeState(player.pmsm.idlingState);
    }
}
