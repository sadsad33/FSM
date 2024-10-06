using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : IState {
    protected PlayerManager player;
    protected float currentMovingSpeed;
    protected Vector3 moveDirection;
    protected Vector3 lookingDirection;

    protected float moveSpeedModifier;

    protected float verticalInput;
    protected float horizontalInput;
    protected float moveAmount;
    protected float mouseXInput;
    protected float mouseYInput;

    protected bool isBottomGrounded;
    public virtual void Enter(CharacterManager character) {
        player = character as PlayerManager;
        Debug.Log("Player Current State : " + GetType());
        Debug.Log("Current State moveDirection : " + moveDirection);
    }

    public virtual void Stay(CharacterManager character) {
        HandleInput();
        //Debug.Log("Movement State Stay 의 MoveDirection : " + moveDirection);
    }

    public virtual void Exit(CharacterManager character) {

    }

    public virtual void HandleInput() {
        float delta = Time.deltaTime;
        HandleGroundCheck();
        HandleYVelocity();
        HandleRotation();
        HandleMovement();
        HandleMouseInput();
        GetWASDInput();
        HandleSprintInput(delta);
        HandleRollInput();
        //Debug.Log("Movement State HandleInput 의 MoveDirection : " + moveDirection);
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

    bool front, back, right, left;
    private void HandleGroundCheck() {
        Vector3 pushingDirection;
        isBottomGrounded = Physics.CheckSphere(player.transform.position, player.GroundCheckSphereRadius, player.groundLayer);
        if (isBottomGrounded) {
            player.isGrounded = true;
        } else {
            pushingDirection = moveDirection;
            HandleEdgeGroundCheck(pushingDirection);
            if (front || back || right || left) {
                player.isGrounded = true;
            } else {
                player.isGrounded = false;
            }
        }
    }

    private void HandleEdgeGroundCheck(Vector3 pushingDirection) {
        if (!player.isGrounded) return;
        front = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), player.transform.forward, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //Debug.Log("Front : " + front);
        back = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), -player.transform.forward, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //Debug.Log("Back : " + back);
        right = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), player.transform.right, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //Debug.Log("Right : " + right);
        left = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), -player.transform.right, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //Debug.Log("Left : " + left);
        HandlePushingPlayerOnEdge(pushingDirection);
    }

    private void HandlePushingPlayerOnEdge(Vector3 pushingDirection) {
        //Debug.Log("절벽에서 밀기");
        //Debug.Log(pushingVelocity);
        if (player.isJumping) return;
        player.cc.Move(pushingDirection * player.pushingForceOnEdge);
    }

    private void HandleYVelocity() {
        if (player.isJumping) return;
        Vector3 tempYVelocity = player.YVelocity;
        if (player.isGrounded) {
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

    protected virtual void HandleRotation() {
        if (player.isPerformingAction) return;
        lookingDirection = CameraManager.instance.cameraTransform.forward * verticalInput;
        lookingDirection += CameraManager.instance.cameraTransform.right * horizontalInput;
        lookingDirection.Normalize();
        lookingDirection.y = 0;

        if (lookingDirection == Vector3.zero) {
            lookingDirection = player.transform.forward;
        }
    }

    protected virtual void HandleMovement() {
        if (player.isJumping) return;
        if (player.isPerformingAction) return;
        moveDirection = CameraManager.instance.myTransform.forward * verticalInput;
        moveDirection += CameraManager.instance.myTransform.right * horizontalInput;
        moveDirection.Normalize();
        Vector3 tempDirection = moveDirection;
        tempDirection.y = 0;
        moveDirection = tempDirection;
    }
}
