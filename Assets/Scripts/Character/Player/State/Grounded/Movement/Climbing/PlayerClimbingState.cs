using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerClimbingState : PlayerMovementState {
        protected float climbingSpeedModifier;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            climbingSpeedModifier = 0.75f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
        }

        protected override void HandleMovement() {
            //Debug.Log("사다리 타기");
            Vector3 moveDirection = Vector3.up * verticalInput;
            moveDirection = moveDirection.normalized;
            float speed = player.moveSpeed * climbingSpeedModifier;
            moveDirection *= speed;
            Debug.Log("Moving Dierction : " + moveDirection);
            if (player.cc.enabled)
                player.cc.Move(moveDirection * Time.deltaTime);
        }
    }
}
