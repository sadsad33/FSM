using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : IState {
    protected PlayerManager player;

    protected Vector3 moveDirection;
    protected Vector3 lookingDirection;
    public Vector3 PlayerMaximumVelocity { get; set; }

    protected float moveSpeedModifier;
    protected bool sprintInputDelaySet;
    protected float sprintInputDelay;

    protected float verticalInput;
    protected float horizontalInput;
    protected float moveAmount;
    protected float mouseXInput;
    protected float mouseYInput;

    protected bool isBottomGrounded;

    public virtual void Enter(CharacterManager character) {
        player = character as PlayerManager;
        Debug.Log("Player Current Movement State : " + GetType());
        //Debug.Log("Current State moveDirection : " + moveDirection);
    }

    public virtual void Stay(CharacterManager character) {
        if (sprintInputDelaySet) {
            sprintInputDelay += Time.deltaTime;
            if (sprintInputDelay < 0.15f) {
                player.playerInputManager.SprintInputTimer = 0f;
            } else {
                sprintInputDelaySet = false;
                sprintInputDelay = 0f;
            }
        }
        //Debug.Log("Delta : " + delta);
        //Debug.Log("SprintInputTimer Traker : " + player.playerInputManager.SprintInputTimer);
        //Debug.Log(player.playerInputManager.SprintInputTimer);
        HandleYVelocity();
        HandleGroundCheck();
        if (player.isPerformingAction) return;
        HandleInput();
        //Debug.Log("Movement State Stay 의 MoveDirection : " + moveDirection);
    }

    public virtual void Exit(CharacterManager character) {

    }

    bool front = false, back = false, right = false, left = false;
    protected void HandleGroundCheck() {
        Vector3 pushingDirection;
        isBottomGrounded = Physics.Raycast(player.transform.position + (Vector3.up * player.bottomGroundCheckRayStartingYPosition), -player.transform.up, player.bottomGroundCheckRayMaxDistance, player.groundLayer);
        if (isBottomGrounded) {
            player.isGrounded = true;
            player.InAirTimer = 0f;
        } else {
            pushingDirection = moveDirection;
            HandleEdgeGroundCheck(pushingDirection);
            if (front || back || right || left) {
                player.isGrounded = true;
                player.InAirTimer = 0f;
            } else {
                player.isGrounded = false;
            }
        }
    }

    protected void HandleEdgeGroundCheck(Vector3 pushingDirection) {
        //if (player.isGrounded) return;
        RaycastHit hit;
        front = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), player.transform.forward, out hit, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //if (front) {
        //    Debug.Log(hit.transform.gameObject);
        //}
        back = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), -player.transform.forward, out hit, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //if (back) {
        //    Debug.Log(hit.transform.gameObject);
        //}
        right = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), player.transform.right, out hit, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //if (right) {
        //    Debug.Log(hit.transform.gameObject);
        //}
        left = Physics.Raycast(player.transform.position + (Vector3.up * player.groundCheckRaycastStartingPosition.y), -player.transform.right, out hit, player.groundCheckRaycastStartingPosition.x, player.groundLayer);
        //if (left) {
        //    Debug.Log(hit.transform.gameObject);
        //}
        HandlePushingPlayerOnEdge(pushingDirection);
    }

    protected void HandlePushingPlayerOnEdge(Vector3 pushingDirection) {
        //Debug.Log("절벽에서 밀기");
        //Debug.Log(pushingVelocity);

        if (player.isJumping) return;
        player.cc.Move(((pushingDirection * player.pushingForceOnEdge) + Vector3.down) * Time.deltaTime);
    }

    protected void HandleYVelocity() {
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

    public virtual void HandleInput() {
        float delta = Time.deltaTime;
        HandleRotation();
        HandleMovement();
        HandleMouseInput();
        GetWASDInput();
        HandleSprintInput(delta);
        HandleRollInput();
        //Debug.Log("Movement State HandleInput 의 MoveDirection : " + moveDirection);
    }

    private void GetWASDInput() {
        //if (player.isPerformingAction) return;
        verticalInput = player.playerInputManager.MovementInput.y;
        horizontalInput = player.playerInputManager.MovementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
    }

    private void HandleMouseInput() {
        mouseXInput = player.playerInputManager.CameraInput.x;
        mouseYInput = player.playerInputManager.CameraInput.y;
    }

    private void HandleSprintInput(float delta) {
        //if (player.isPerformingAction) return;
        if (player.isJumping) return;
        if (player.playerInputManager.SprintInput) {
            player.playerInputManager.SprintInputTimer += delta;
        }
    }

    private void HandleRollInput() {
        //if (player.isPerformingAction) return;
        if (player.playerInputManager.SprintInput) {
            player.playerInputManager.PlayerInput.PlayerActions.Sprint.canceled += i => player.playerInputManager.RollFlag = true;
            //if (player.playerInputManager.SprintInputFalse == null) 
            //    player.playerInputManager.PlayerInput.PlayerActions.Sprint.canceled += i => player.playerInputManager.SprintInputFalse = Time.deltaTime;
        }
    }

    protected virtual void HandleRotation() {
        //if (player.isPerformingAction) return;
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
        //if (player.isPerformingAction) return;
        moveDirection = CameraManager.instance.myTransform.forward * verticalInput;
        moveDirection += CameraManager.instance.myTransform.right * horizontalInput;
        moveDirection.Normalize();
        Vector3 tempDirection = moveDirection;
        tempDirection.y = 0;
        moveDirection = tempDirection;
    }
}
