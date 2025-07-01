using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerMovementState : CharacterMovementState {
        protected PlayerManager player;

        //protected Vector3 moveDirection;
        //protected Vector3 lookingDirection;
        //public Vector3 PlayerMaximumVelocity { get; set; }

        //protected float moveSpeedModifier;
        protected bool sprintInputDelaySet;
        protected float sprintInputDelay;

        protected float verticalInput;
        protected float horizontalInput;
        protected float moveAmount;
        protected float mouseXInput;
        protected float mouseYInput;

        //protected bool isBottomGrounded;

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            moveDirection = Vector3.zero;
            player = character as PlayerManager;
            Debug.Log("Player Current Movement State : " + GetType());
            //Debug.Log("Current State moveDirection : " + moveDirection);
        }

        public override void Stay(CharacterManager character) {
            if (!player.consumingStamina && player.staminaRegenerateTimer >= 2f)
                player.playerStatsManager.RegenerateStamina(0.15f);
            if (sprintInputDelaySet) {
                sprintInputDelay += Time.deltaTime;
                if (sprintInputDelay < 0.15f) {
                    player.playerInputManager.SprintInputTimer = 0f;
                } else {
                    sprintInputDelaySet = false;
                    sprintInputDelay = 0f;
                }
            }

            //player.pasm.GetCurrentState().Stay(player);
            base.Stay(character);
            //HandleYVelocity();
            //HandleGroundCheck();
            //if (player.isPerformingAction) return;
            HandleInput();
        }

        public override void Exit(CharacterManager character) {

        }
        public virtual void HandleInput() {
            float delta = Time.deltaTime;
            // 달리기, 질주 상태에서 멈췄을 경우
            // stopping 상태에서 더이상 아무런 입력이 없으면 애니메이션을 끝까지 재생
            if (player.isPerformingAction) return;
            GetWASDInput();
            HandleRotation();
            HandleMovement();
            HandleMouseInput();
            HandleSprintInput(delta);
            HandleRollInput();
            //Debug.Log("Movement State HandleInput 의 MoveDirection : " + moveDirection);
        }

        private void GetWASDInput() {
            verticalInput = player.playerInputManager.MovementInput.y;
            if (player.playerInteractionManager.isInteracting) verticalInput = 0;
            horizontalInput = player.playerInputManager.MovementInput.x;
            if (player.playerInteractionManager.isInteracting) horizontalInput = 0;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        }

        private void HandleMouseInput() {
            mouseXInput = player.playerInputManager.CameraInput.x;
            mouseYInput = player.playerInputManager.CameraInput.y;
        }

        private void HandleSprintInput(float delta) {
            if (player.isJumping) return;
            if (player.playerInputManager.SprintInput) {
                player.playerInputManager.SprintInputTimer += delta;
            }
        }

        private void HandleRollInput() {
            if (player.playerInputManager.SprintInput) {
                player.playerInputManager.PlayerInput.PlayerActions.Sprint.canceled += i => player.playerInputManager.RollFlag = true;
            }
        }

        protected virtual void HandleRotation() {
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
            moveDirection = CameraManager.instance.myTransform.forward * verticalInput;
            moveDirection += CameraManager.instance.myTransform.right * horizontalInput;
            moveDirection.y = 0;
            moveDirection.Normalize();
            //Vector3 tempDirection = moveDirection;
            //tempDirection.y = 0;
            //moveDirection = tempDirection;
        }
    }
}