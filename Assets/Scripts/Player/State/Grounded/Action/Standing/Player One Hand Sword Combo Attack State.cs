using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHandSwordComboAttackState : PlayerActionState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.canDoComboAttack = false;
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("OH_Sword_Attack2", player.isPerformingAction);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        HandleComboAttack();
        HandleRotation();
    }

    public override void Exit(CharacterManager character) {
        base.Exit(character);
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isPerformingAction) {
            player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }
    }

    private void HandleComboAttack() {
        if (player.canDoComboAttack && player.playerInputManager.LightAttackInput)
            player.pasm.ChangeState(player.pasm.oneHandSwordFinalAttackState);
    }

    private void HandleRotation() {
        if (!player.canRotateDuringAction) return;
        lookingDirection = CameraManager.instance.cameraTransform.forward * verticalInput;
        lookingDirection += CameraManager.instance.cameraTransform.right * horizontalInput;
        lookingDirection.Normalize();
        lookingDirection.y = 0;

        if (lookingDirection == Vector3.zero) {
            lookingDirection = player.transform.forward;
        }
        Quaternion tr = Quaternion.LookRotation(lookingDirection);
        Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
        player.transform.rotation = targetRotation;
    }
}
