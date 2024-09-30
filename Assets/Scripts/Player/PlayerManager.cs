using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerInputManager playerInputManager;
    public PlayerMovementStateMachine pmsm;

    public float InAirTimer { get; set; }
    public Vector3 YVelocity { get; set; }
    public float GroundedYVelocity { get; set; }
    public float GravityForce { get; set; }
    public float FallStartYVelocity { get; set; }
    public bool FallingVelocitySet { get; set; }

    public float GroundCheckSphereRadius { get; set; }

    public LayerMask groundLayer;
    
    protected override void Awake() {
        base.Awake();
        PlayerInit();
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
        float delta = Time.deltaTime;
        CameraManager.instance.FollowTarget(delta);
        CameraManager.instance.HandleCameraRotation(delta, playerInputManager.CameraInput.x, playerInputManager.CameraInput.y);
    }

    protected void LateUpdate() {
        playerAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
    }

    private void PlayerInit() {
        InAirTimer = 0f;
        YVelocity = Vector3.zero;
        GroundedYVelocity = -10f;
        GravityForce = -10f;
        FallStartYVelocity = -3f;
        FallingVelocitySet = false;
        GroundCheckSphereRadius = 0.4f;
    }
}
