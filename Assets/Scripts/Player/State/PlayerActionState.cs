using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerActionState : IState {
        protected PlayerManager player;
        protected Vector3 lookingDirection;
        protected float verticalInput;
        protected float horizontalInput;
        protected float moveAmount;
        public virtual void Enter(CharacterManager character) {
            player = character as PlayerManager;
            //Debug.Log("Player Current Action State : " + GetType());
        }

        public virtual void Stay(CharacterManager character) {
            HandleRotationInput();
            if (player.isPerformingAction) return;
            HandleInput();
        }

        public virtual void Exit(CharacterManager character) {

        }

        public virtual void HandleInput() {
            float delta = Time.deltaTime;
        }

        protected virtual void HandleRotationInput() {
            if (!player.canRotateDuringAction) return;
            verticalInput = player.playerInputManager.MovementInput.y;
            horizontalInput = player.playerInputManager.MovementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        }
    }
}