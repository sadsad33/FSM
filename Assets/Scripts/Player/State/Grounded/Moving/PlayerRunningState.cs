using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerMovingState {
   
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerInputManager.SprintInputTimer = 0f;
        moveSpeedModifier = 1f;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        Debug.Log("Running State Stay ÀÇ MoveDirection : " + moveDirection);
        player.RunningStateTimer += Time.deltaTime;
        //HandleMovement();
        player.playerAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        Debug.Log("Running State HandleInput ÀÇ MoveDirection : " + moveDirection);
        if (moveAmount <= 0f) {
            if (player.RunningStateTimer >= 0.5f)
                player.pmsm.ChangeState(player.pmsm.lightStoppingState);
            else
                player.pmsm.ChangeState(player.pmsm.idlingState);
        }
        else {
            if (player.playerInputManager.WalkInput) player.pmsm.ChangeState(player.pmsm.walkingState);
            if (player.playerInputManager.SprintInput && player.playerInputManager.SprintInputTimer >= 0.3f) player.pmsm.ChangeState(player.pmsm.sprintingState);
        }
    }

    protected override void HandleRotation() {
        base.HandleRotation();
        Quaternion tr = Quaternion.LookRotation(lookingDirection);
        Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
        player.transform.rotation = targetRotation;
    }

    protected override void HandleMovement() {
        //Debug.Log("Move Direction Before Call Base : " + moveDirection);
        base.HandleMovement();
        //Debug.Log("Move Direction After Call Base : " + moveDirection);
        float speed = player.moveSpeed * moveSpeedModifier;
        currentMovingSpeed = speed;
        moveDirection *= speed;
        player.cc.Move(moveDirection / 200f);
    }
}
