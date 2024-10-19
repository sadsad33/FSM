using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerAnimatorManager.PlayAnimation("Medium Stop", player.isPerformingAction);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        //player.playerInputManager.SprintInputTimer = 0f;
        //if (moveAmount > 0) player.isPerformingAction = false;
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isPerformingAction)
            player.pmsm.ChangeState(player.pmsm.idlingState);
    }
}
