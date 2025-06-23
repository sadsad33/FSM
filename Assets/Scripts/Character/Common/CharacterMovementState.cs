using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterMovementState : IState {
        protected CharacterManager character;
        protected Vector3 moveDirection;
        protected Vector3 lookingDirection;
        //public Vector3 CharacterMaximumVelocity { get; set; }

        protected float moveSpeedModifier;
        protected bool isBottomGrounded;

        public virtual void Enter(CharacterManager character) {
            this.character = character;
        }

        public virtual void Stay(CharacterManager character) {
            HandleYVelocity();
            HandleGroundCheck();
        }

        public virtual void Exit(CharacterManager character) {
        }

        bool front = false, back = false, right = false, left = false;
        protected void HandleGroundCheck() {
            if (character.isClimbing) return;
            Vector3 pushingDirection = Vector3.zero;
            isBottomGrounded = Physics.Raycast(character.transform.position + (Vector3.up * character.bottomGroundCheckRayStartingYPosition), -character.transform.up, character.bottomGroundCheckRayMaxDistance, character.groundLayer);
            if (isBottomGrounded) {
                character.isGrounded = true;
                character.InAirTimer = 0f;
            } else {
                pushingDirection += moveDirection;
                HandleEdgeGroundCheck(pushingDirection);
                if (front || back || right || left) {
                    character.isGrounded = true;
                    character.InAirTimer = 0f;
                } else {
                    character.isGrounded = false;
                }
            }
        }

        protected void HandleEdgeGroundCheck(Vector3 pushingDirection) {
            RaycastHit hit;
            front = Physics.Raycast(character.transform.position + (Vector3.up * character.groundCheckRaycastStartingPosition.y), character.transform.forward, out hit, character.groundCheckRaycastStartingPosition.x, character.groundLayer);
            back = Physics.Raycast(character.transform.position + (Vector3.up * character.groundCheckRaycastStartingPosition.y), -character.transform.forward, out hit, character.groundCheckRaycastStartingPosition.x, character.groundLayer);
            right = Physics.Raycast(character.transform.position + (Vector3.up * character.groundCheckRaycastStartingPosition.y), character.transform.right, out hit, character.groundCheckRaycastStartingPosition.x, character.groundLayer);
            left = Physics.Raycast(character.transform.position + (Vector3.up * character.groundCheckRaycastStartingPosition.y), -character.transform.right, out hit, character.groundCheckRaycastStartingPosition.x, character.groundLayer);
            HandlePushingPlayerOnEdge(pushingDirection);
        }

        protected void HandlePushingPlayerOnEdge(Vector3 pushingDirection) {
            //Debug.Log("절벽에서 밀기");
            if (character.isJumping) return;
            if (character.cc.enabled)
                character.cc.Move(((pushingDirection * character.pushingForceOnEdge) + Vector3.down) * Time.deltaTime);
        }

        protected void HandleYVelocity() {
            if (character.isJumping || character.isClimbing) return;
            Vector3 tempYVelocity = character.YVelocity;
            if (character.isGrounded) {
                character.FallingVelocitySet = false;
                tempYVelocity.y = character.GroundedYVelocity;
                character.YVelocity = tempYVelocity;
            } else {
                if (!character.FallingVelocitySet) {
                    character.FallingVelocitySet = true;
                    tempYVelocity.y = character.FallStartYVelocity;
                    character.YVelocity = tempYVelocity;
                }
                character.InAirTimer += Time.deltaTime;
                tempYVelocity.y += character.GravityForce * Time.deltaTime;
                character.YVelocity = tempYVelocity;
            }
            character.characterAnimatorManager.animator.SetFloat("inAirTimer", character.InAirTimer);
            if (character.cc.enabled)
                character.cc.Move(character.YVelocity * Time.deltaTime);
        }
    }
}
