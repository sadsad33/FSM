using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandToCrouchState : PlayerGroundedState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        //Debug.Log(player.isCrouched);
        player.isCrouched = true;
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("Standing To Crouched", false);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.cc.Move(Vector3.zero);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        //if (!player.isPerformingAction)
        player.pmsm.ChangeState(player.pmsm.crouchedIdlingState);
    }
}
