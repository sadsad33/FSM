using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState {

    protected Vector3 aeroInputDirection;
    public Vector3 MovingVelocityInAir { get; set; } // 공중에 떠 있는 상태에서의 이동방향
    public override void Enter(CharacterManager character) {
        base.Enter(character);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        //Debug.Log("MovingVelocityInAir Update : " + MovingVelocityInAir);
        //Debug.Log("Airborne State Stay 의 MoveDirection : " + moveDirection);
    }

    public override void Exit(CharacterManager character) {

    }

    public override void HandleInput() {
        base.HandleInput();
        //Debug.Log("Airborne State HandleInput 의 MoveDirection : " + moveDirection);
    }

    protected override void HandleRotation() {
        base.HandleRotation();
        //Debug.Log("Looking Direction : " + lookingDirection);
        //Debug.Log("공중에서 회전");
        Quaternion tr = Quaternion.LookRotation(lookingDirection);
        Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, (player.rotationSpeed * 0.1f) * Time.deltaTime);
        player.transform.rotation = targetRotation;
    }

    protected override void HandleMovement() {
        base.HandleMovement();
        //Debug.Log(MovingDirectionInAir);
        aeroInputDirection = CameraManager.instance.myTransform.forward * verticalInput;
        aeroInputDirection += CameraManager.instance.myTransform.right * horizontalInput;
        aeroInputDirection.Normalize();
        aeroInputDirection.y = 0;
        moveDirection = MovingVelocityInAir;
        Vector3 targetDirection = moveDirection + aeroInputDirection;
        //if (player.pmsm.GetCurrentState() == player.pmsm.runningJumpState) Debug.Log("RunningJump Target Velocity : " + targetDirection);
        //if (player.pmsm.GetCurrentState() == player.pmsm.fallingState) Debug.Log("Falling Target Velocity : " + targetDirection);
        if (aeroInputDirection == Vector3.zero) player.cc.Move(targetDirection / 350f);
        else player.cc.Move(targetDirection * Time.deltaTime);
    }

}