using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterManager : MonoBehaviour {
        public CharacterInteractionManager characterInteractionManager;
        public CharacterAnimatorManager characterAnimatorManager;
        public CharacterEffectsManager characterEffectsManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterController cc;
        public Rigidbody rigidbody;

        public bool gotHit;
        public float hitTimer;

        public bool isInvulnerable;
        public bool isGrounded;
        public bool isJumping;
        public bool isCrouching;

        public bool isParrying;
        public bool hasBeenParried;
        public bool canBeRiposted;
        public bool beingRiposted;
        public bool beingBackstabbed;

        public bool isClimbing;
        public bool isOnLadderTopEdge;
        public bool isOnLadderBottomEdge;

        public bool isPerformingAction;
        public bool isAttacking;
        public bool isMoving;
        public bool isTwoHanding;

        public bool canAttackDuringAction;
        public bool canRotateDuringAction;

        public bool canSliding;
        public bool canDoComboAttack;

        public float moveSpeed;
        public float rotationSpeed;

        [SerializeField] Transform rightFoot;
        [SerializeField] Transform leftFoot;
        public bool rightFootUp;

        //public Transform temp;
        #region Ariborne
        public float InAirTimer { get; set; }
        protected float prevYPosition;
        protected float deltaYPosition;

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

        public StateMachine csm;
        protected virtual void Awake() {
            csm = new(this);
            cc = GetComponent<CharacterController>();
            characterInteractionManager = GetComponentInChildren<CharacterInteractionManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Start() {

        }

        protected virtual void Update() {
            if (gotHit) {
                if (hitTimer >= 0.3f) {
                    gotHit = false;
                    hitTimer = 0.0f;
                }
                hitTimer += Time.deltaTime;
            }

            if (rightFoot.position.y > leftFoot.position.y) rightFootUp = true;
            else rightFootUp = false;

            if (!isClimbing) {
                float curYPosition = transform.position.y;
                deltaYPosition = curYPosition - prevYPosition;
                if (deltaYPosition <= -0.02) {
                    //Debug.Log("���� ����");
                    isGrounded = false;
                }
                prevYPosition = curYPosition;
            }
        }

        protected virtual void CharacterInit() {
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
    }
}