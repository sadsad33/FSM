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
        //Debug.Log("Running Jump State Stay 의 MoveDirection : " + moveDirection);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        HandleJumping();
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        //Debug.Log("Running Jump State HandleInput 의 MoveDirection : " + moveDirection);
        if (player.transform.position.y >= maximumHeight) {
            player.isJumping = false;
            //Debug.Log("기존 공중 속도 : " + MovingVelocityInAir.normalized);
            //Debug.Log("공중 입력에 따른 속도 : " + aeroInputDirection.normalized);
            //float dotProduct = Vector3.Dot(MovingVelocityInAir.normalized, aeroInputDirection.normalized);
            //if (dotProduct == 1 || aeroInputDirection == Vector3.zero) player.pmsm.fallingState.MovingVelocityInAir = aeroInputDirection;
            //else {
            //    player.pmsm.fallingState.MovingVelocityInAir = (MovingVelocityInAir + aeroInputDirection).normalized;
            //}
            player.pmsm.fallingState.MovingVelocityInAir = (MovingVelocityInAir + aeroInputDirection).normalized;
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
