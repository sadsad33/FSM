using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandToMoveState : PlayerLandingState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        //Debug.Log(player.isPerformingAction);
        player.playerAnimatorManager.PlayAnimation("Land To Move", player.isPerformingAction);
    }

    public override void Stay(CharacterManager character) {
        //Debug.Log(player.isPerformingAction);
        base.Stay(character);
        //player.cc.Move(Vector3.zero);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        if (player.isPerformingAction) return;
        base.HandleInput();
        if (player.playerInputManager.SprintInput)
            player.pmsm.ChangeState(player.pmsm.sprintingState);
        else if (player.playerInputManager.WalkInput)
            player.pmsm.ChangeState(player.pmsm.walkingState);
        else
            player.pmsm.ChangeState(player.pmsm.runningState);
            
    }
}
