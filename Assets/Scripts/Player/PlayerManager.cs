using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager {
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerInputManager playerInputManager;
    public PlayerMovementStateMachine pmsm;
    public PlayerActionStateMachine pasm;

    #region Ariborne
    public float InAirTimer { get; set; }
    private float prevYPosition;
    private float deltaYPosition;

    #region Falling
    public Vector2 groundCheckRaycastStartingPosition = Vector2.zero;
    public float pushingForceOnEdge;
    public float bottomGroundCheckRayStartingYPosition;
    public float bottomGroundCheckRayMaxDistance;
    public Vector3 YVelocity { get; set; }
    public float GroundedYVelocity { get; set; }
    public float GravityForce { get; set; }
    public float FallStartYVelocity { get; set; }
    public bool FallingVelocitySet { get; set; }
    public float GroundCheckSphereRadius { get; set; }
    #endregion

    #region Jumping
    public float MaximumJumpHeight { get; set; }
    public float JumpStartYVelocity { get; set; }
    public float JumpForce { get; set; }
    #endregion

    #endregion

    public float RunningStateTimer { get; set; }

    public LayerMask groundLayer;

    protected override void Awake() {
        base.Awake();
        PlayerInit();
        prevYPosition = transform.position.y;
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
        pmsm = new PlayerMovementStateMachine(this);
        pasm = new PlayerActionStateMachine(this);
        CameraManager.instance.AssignCameraToPlayer(this);
    }

    protected override void Start() {
        pmsm.ChangeState(pmsm.idlingState);
        pasm.ChangeState(pasm.standingActionIdlingState);
    }

    protected override void Update() {
        //Debug.Log(playerInputManager.MovementInput);
        //if (!isPerformingAction)
        //HandleGroundCheck();
        //HandleYVelocity();
        pmsm.GetCurrentState().Stay(this);
        pasm.GetCurrentState().Stay(this);
        //if (!isPerformingAction)

        float curYPosition = transform.position.y;
        deltaYPosition = curYPosition - prevYPosition;
        if (deltaYPosition <= -0.02) {
            //Debug.Log("강제 낙하");
            isGrounded = false;
        }
        prevYPosition = curYPosition;

        float delta = Time.deltaTime;
        CameraManager.instance.FollowTarget(delta);
        CameraManager.instance.HandleCameraRotation(delta, playerInputManager.CameraInput.x, playerInputManager.CameraInput.y);
    }

    protected void LateUpdate() {
        playerAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
        playerAnimatorManager.animator.SetBool("isGrounded", isGrounded);
        playerAnimatorManager.animator.SetBool("isCrouched", isCrouched);
        playerInputManager.LightAttackInput = false;
        playerInputManager.JumpInput = false;
        playerInputManager.RollFlag = false;
    }

    private void PlayerInit() {
        InAirTimer = 0f;
        YVelocity = Vector3.zero;
        GroundedYVelocity = -10f;
        GravityForce = -10f;
        FallStartYVelocity = -1.5f;
        FallingVelocitySet = false;
        GroundCheckSphereRadius = 0.3f;

        MaximumJumpHeight = 1.5f;
        JumpStartYVelocity = 2.5f;
        JumpForce = 1f;
    }

    //bool front = false, back = false, right = false, left = false;
    //protected void HandleGroundCheck() {
    //    Vector3 pushingDirection;
    //    isBottomGrounded = Physics.Raycast(transform.position + (Vector3.up * bottomGroundCheckRayStartingYPosition), -transform.up, bottomGroundCheckRayMaxDistance, groundLayer);
    //    if (isBottomGrounded) {
    //        isGrounded = true;
    //        InAirTimer = 0f;
    //    } else {
    //        pushingDirection = MoveDirection;
    //        HandleEdgeGroundCheck(pushingDirection);
    //        if (front || back || right || left) {
    //            isGrounded = true;
    //            InAirTimer = 0f;
    //        } else {
    //            isGrounded = false;
    //        }
    //    }
    //}

    //protected void HandleEdgeGroundCheck(Vector3 pushingDirection) {
    //    //if (player.isGrounded) return;
    //    RaycastHit hit;
    //    front = Physics.Raycast(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.forward, out hit, groundCheckRaycastStartingPosition.x, groundLayer);
    //    //if (front) {
    //    //    Debug.Log(hit.transform.gameObject);
    //    //}
    //    back = Physics.Raycast(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.forward, out hit, groundCheckRaycastStartingPosition.x, groundLayer);
    //    //if (back) {
    //    //    Debug.Log(hit.transform.gameObject);
    //    //}
    //    right = Physics.Raycast(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.right, out hit, groundCheckRaycastStartingPosition.x, groundLayer);
    //    //if (right) {
    //    //    Debug.Log(hit.transform.gameObject);
    //    //}
    //    left = Physics.Raycast(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.right, out hit, groundCheckRaycastStartingPosition.x, groundLayer);
    //    //if (left) {
    //    //    Debug.Log(hit.transform.gameObject);
    //    //}
    //    HandlePushingPlayerOnEdge(pushingDirection);
    //}

    //protected void HandlePushingPlayerOnEdge(Vector3 pushingDirection) {
    //    //Debug.Log("절벽에서 밀기");
    //    //Debug.Log(pushingVelocity);

    //    if (isJumping) return;
    //    cc.Move(((pushingDirection * pushingForceOnEdge) + Vector3.down) * Time.deltaTime);
    //}

    //protected void HandleYVelocity() {
    //    if (isJumping) return;
    //    Vector3 tempYVelocity = YVelocity;
    //    if (isGrounded) {
    //        FallingVelocitySet = false;
    //        tempYVelocity.y = GroundedYVelocity;
    //        YVelocity = tempYVelocity;
    //    } else {
    //        if (!FallingVelocitySet) {
    //            FallingVelocitySet = true;
    //            tempYVelocity.y = FallStartYVelocity;
    //            YVelocity = tempYVelocity;
    //        }
    //        InAirTimer += Time.deltaTime;
    //        tempYVelocity.y += GravityForce * Time.deltaTime;
    //        YVelocity = tempYVelocity;
    //    }
    //    playerAnimatorManager.animator.SetFloat("inAirTimer", InAirTimer);
    //    cc.Move(YVelocity * Time.deltaTime);
    //}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        //Gizmos.DrawSphere(transform.position, GroundCheckSphereRadius);
        Gizmos.DrawRay(transform.position + (Vector3.up * bottomGroundCheckRayStartingYPosition), -transform.up * bottomGroundCheckRayMaxDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.forward * groundCheckRaycastStartingPosition.x);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.forward * groundCheckRaycastStartingPosition.x);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.right * groundCheckRaycastStartingPosition.x);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.right * groundCheckRaycastStartingPosition.x);
    }
}
