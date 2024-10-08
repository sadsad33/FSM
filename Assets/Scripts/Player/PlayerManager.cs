using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerInputManager playerInputManager;
    public PlayerMovementStateMachine pmsm;

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
        CameraManager.instance.AssignCameraToPlayer(this);
    }

    protected override void Start() {
        pmsm.ChangeState(pmsm.idlingState);
    }

    protected override void Update() {
        pmsm.GetCurrentState().Stay(this);
        
        float curYPosition = transform.position.y;
        deltaYPosition = curYPosition - prevYPosition;
        if (deltaYPosition < -0.01) isGrounded = false;
        prevYPosition = curYPosition;

        float delta = Time.deltaTime;
        CameraManager.instance.FollowTarget(delta);
        CameraManager.instance.HandleCameraRotation(delta, playerInputManager.CameraInput.x, playerInputManager.CameraInput.y);
    }

    protected void LateUpdate() {
        playerAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
        playerAnimatorManager.animator.SetBool("isGrounded", isGrounded);
        playerInputManager.JumpInput = false;
        playerInputManager.RollFlag = false;
    }

    private void PlayerInit() {
        InAirTimer = 0f;
        YVelocity = Vector3.zero;
        GroundedYVelocity = -10f;
        GravityForce = -10f;
        FallStartYVelocity = -2.5f;
        FallingVelocitySet = false;
        GroundCheckSphereRadius = 0.3f;

        MaximumJumpHeight = 1.5f;
        JumpStartYVelocity = 2.5f;
        JumpForce = 1f;
    }

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
 