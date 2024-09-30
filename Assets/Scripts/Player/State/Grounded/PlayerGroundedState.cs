using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerMovementState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        
    }

    public override void HandleInput() {
        base.HandleInput();
        if (player.playerInputManager.RollFlag) {
            if (player.playerInputManager.SprintInputTimer > 0f && player.playerInputManager.SprintInputTimer < 0.3f) {
                if (!player.isPerformingAction) {
                    player.pmsm.ChangeState(player.pmsm.rollingState);
                }
            }
        }
                
    }
}
