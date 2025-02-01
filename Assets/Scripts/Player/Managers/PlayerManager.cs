using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerManager : CharacterManager {
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerInputManager playerInputManager;
        
        public PlayerMovementStateMachine pmsm;
        public PlayerActionStateMachine pasm;
        
        public PlayerStatsManager playerStatsManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public PlayerInteractionManager playerInteractionManager;

        public Transform temp;
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
        
        public bool consumingStamina;
        public float staminaRegenerateTimer;

        protected override void Awake() {
            base.Awake();
            PlayerInit();
            prevYPosition = transform.position.y;
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerInteractionManager = GetComponentInChildren<PlayerInteractionManager>();
            pmsm = new PlayerMovementStateMachine(this);
            pasm = new PlayerActionStateMachine(this);
            CameraManager.instance.AssignCameraToPlayer(this);
        }

        protected override void Start() {
            pmsm.ChangeState(pmsm.idlingState);
            //pasm.ChangeState(pasm.standingActionIdlingState);
        }

        protected override void Update() {
            base.Update();
            pmsm.GetCurrentState().Stay(this);
            //pasm.GetCurrentState().Stay(this);
            
            if (playerInputManager.RightWeaponChangeInput) {
                playerEquipmentManager.ChangeRightHandWeapon();
            }

            if (playerInputManager.MenuSelectionInput) {
                PlayerUIManager.instance.HandleESCInput();
            }

            if (!isClimbing) {
                float curYPosition = transform.position.y;
                deltaYPosition = curYPosition - prevYPosition;
                if (deltaYPosition <= -0.02) {
                    //Debug.Log("°­Á¦ ³«ÇÏ");
                    isGrounded = false;
                }
                prevYPosition = curYPosition;
            }

            if (!consumingStamina && staminaRegenerateTimer < 2f) {
                staminaRegenerateTimer += Time.deltaTime;
            }

            float delta = Time.deltaTime;
            CameraManager.instance.FollowTarget(delta);
            CameraManager.instance.HandleCameraRotation(delta, playerInputManager.CameraInput.x, playerInputManager.CameraInput.y);

            if (playerInputManager.UIAction1) {
                playerAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.01f, Time.deltaTime);
                transform.position = Vector3.Slerp(transform.position, temp.position, 5 * Time.deltaTime);
            }
        }

        protected void LateUpdate() {
            playerAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
            playerAnimatorManager.animator.SetBool("isAttacking", isAttacking);
            playerAnimatorManager.animator.SetBool("isGrounded", isGrounded);
            playerAnimatorManager.animator.SetBool("isCrouched", isCrouched);
            playerAnimatorManager.animator.SetBool("isClimbing", isClimbing);
            playerAnimatorManager.animator.SetBool("rightFootUp", rightFootUp);
            playerInputManager.LightAttackInput = false;
            playerInputManager.HeavyAttackInput = false;
            playerInputManager.JumpInput = false;
            playerInputManager.RollFlag = false;
            playerInputManager.RightWeaponChangeInput = false;
            playerInputManager.LeftWeaponChangeInput = false;
            playerInputManager.MenuSelectionInput = false;
            playerInputManager.InteractionInput = false;
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
}