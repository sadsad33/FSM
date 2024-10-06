using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningJumpState : PlayerAirborneState
{
    private float maximumHeight;
    Vector3 playerYVelocity;
    public Vector3 moveDirectionInRunningJumpState;
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isJumping = true;
        maximumHeight = player.transform.position.y + player.MaximumJumpHeight;
        playerYVelocity = player.YVelocity;
        playerYVelocity.y = player.JumpStartYVelocity;
        player.YVelocity = playerYVelocity;
        player.playerAnimatorManager.PlayAnimation("Running Jump", false);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        //Debug.Log("Running Jump State Stay ÀÇ MoveDirection : " + moveDirection);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        HandleJumping();
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        //Debug.Log("Running Jump State HandleInput ÀÇ MoveDirection : " + moveDirection);
        if (player.transform.position.y >= maximumHeight) {
            player.isJumping = false;
            player.pmsm.fallingState.MovingDirectionInAir = player.pmsm.runningJumpState.MovingDirectionInAir;
            player.pmsm.ChangeState(player.pmsm.fallingState);
        }
    }

    private void HandleJumping() {
        if (player.transform.position.y < maximumHeight) {
            Vector3 tempVelocity = player.YVelocity;
            tempVelocity.y += player.JumpForce * Time.deltaTime;
            player.YVelocity = tempVelocity;
            player.cc.Move(player.YVelocity * Time.deltaTime);
        }
    }

    
}
