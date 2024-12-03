using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerMovingState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerStatsManager.DeductStamina(25f);
        player.consumingStamina = true;
        moveSpeedModifier = 2f;
        player.pasm.ChangeState(player.pasm.slidingActionIdlingState);
        player.isMoving = true;
        player.canSliding = false;
        player.playerAnimatorManager.PlayAnimation("Sliding", player.isMoving);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
        player.isMoving = false;
        player.consumingStamina = false;
        player.staminaRegenerateTimer = 0f;
    }

    public override void HandleInput() {
        base.HandleInput();
        //if (!player.isPerformingAction)
        if (!player.isMoving) {
            player.pmsm.ChangeState(player.pmsm.idlingState);
            player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }
    }
}
