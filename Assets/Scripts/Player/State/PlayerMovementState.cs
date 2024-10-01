using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : IState
{
    protected PlayerManager player;

    protected Vector3 moveDirection;
    
    protected float moveSpeedModifier;

    protected float verticalInput;
    protected float horizontalInput;
    protected float moveAmount;
    protected float mouseXInput;
    protected float mouseYInput;

    public virtual void Enter(CharacterManager character) {
        player = character as PlayerManager;
        Debug.Log("Player Current State : " + GetType());
    }

    public virtual void Stay(CharacterManager character) {
        HandleInput();
    }

    public virtual void Exit(CharacterManager character) {
        
    }

    public virtual void HandleInput() {
        float delta = Time.deltaTime;
        HandleGroundCheck();
        HandleYVelocity();
        HandleGroundedRotation();
        HandleMouseInput();
        GetWASDInput();
        HandleSprintInput(delta);
        HandleRollInput();
    }

    private void GetWASDInput() {
        verticalInput = player.playerInputManager.MovementInput.y;
        horizontalInput = player.playerInputManager.MovementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
    }

    private void HandleMouseInput() {
        mouseXInput = player.playerInputManager.CameraInput.x;
        mouseYInput = player.playerInputManager.CameraInput.y;
    }

    private void HandleSprintInput(float delta) {
        if (player.isPerformingAction) return;
        if (player.playerInputManager.SprintInput) {
            player.playerInputManager.SprintInputTimer += delta;
        }
    }

    private void HandleRollInput() {
        if (player.playerInputManager.SprintInput) {
            player.playerInputManager.PlayerInput.PlayerActions.Sprint.canceled += i => player.playerInputManager.RollFlag = true;
        }
    }

    private void HandleGroundCheck() {
        player.isGrounded = Physics.CheckSphere(player.transform.position, player.GroundCheckSphereRadius, player.groundLayer);
    }

    private void HandleYVelocity() {
        Vector3 tempYVelocity = player.YVelocity;
        if (player.isGrounded) {
            player.InAirTimer = 0;
            player.FallingVelocitySet = false;
            tempYVelocity.y = player.GroundedYVelocity;
            player.YVelocity = tempYVelocity;
        } else {
            if (!player.FallingVelocitySet) {
                player.FallingVelocitySet = true;
                tempYVelocity.y = player.FallStartYVelocity;
                player.YVelocity = tempYVelocity;
            }
            player.InAirTimer += Time.deltaTime;
            tempYVelocity.y += player.GravityForce * Time.deltaTime;
            player.YVelocity = tempYVelocity;
        }
        player.playerAnimatorManager.animator.SetFloat("inAirTimer", player.InAirTimer);
        player.cc.Move(player.YVelocity * Time.deltaTime);
    }

    protected void HandleGroundedRotation() {
        if (!player.isGrounded) return;
        if (player.isPerformingAction) return;
        Vector3 targetDir;
        targetDir = CameraManager.instance.cameraTransform.forward * verticalInput;
        targetDir += CameraManager.instance.cameraTransform.right * horizontalInput;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero) {
            targetDir = player.transform.forward;
        }

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
        player.transform.rotation = targetRotation;
    }

    protected virtual void HandleGroundedMovement() {
        if (player.isPerformingAction) return;
        moveDirection = CameraManager.instance.myTransform.forward * verticalInput;
        moveDirection += CameraManager.instance.myTransform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
    }
}
