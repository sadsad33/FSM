using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerMovingState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        moveSpeedModifier = 2f;
        player.canSliding = false;
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("Sliding", player.isPerformingAction);
        //Debug.Log(player.canSliding);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        //if (!player.isPerformingAction)
        player.pmsm.ChangeState(player.pmsm.idlingState);
    }
}
