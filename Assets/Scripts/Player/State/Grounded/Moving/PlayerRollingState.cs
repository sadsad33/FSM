using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerMovingState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerAnimatorManager.PlayAnimation("Rolling", true);
        player.isInvulnerable = true;
        player.playerInputManager.RollFlag = false;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        player.isInvulnerable = false;
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isPerformingAction)
            player.pmsm.ChangeState(player.pmsm.idlingState);
    }
}