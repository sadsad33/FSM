using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInputManager : MonoBehaviour {
        public PlayerInput PlayerInput { get; set; }
        public bool WalkInput { get; set; }
        public bool SprintInput { get; set; }
        public float SprintInputTimer { get; set; }
        public bool CrouchInput { get; set; }
        public Vector2 MovementInput { get; set; }
        public Vector2 CameraInput { get; set; }
        public bool RollFlag { get; set; }
        public bool LightAttackInput { get; set; }
        public bool JumpInput { get; set; }
        public bool HeavyAttackInput { get; set; }
        public bool RightWeaponChangeInput { get; set; }
        public bool LeftWeaponChangeInput { get; set; }
        public bool MenuSelectionInput { get; set; }
        public bool InteractionInput { get; set; }
        public bool UIAction1 { get; set; }

        private void Awake() {
            if (PlayerInput == null)
                PlayerInput = new PlayerInput();
        }

        private void OnEnable() {
            PlayerInput.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();
            PlayerInput.PlayerMovement.Camera.performed += i => CameraInput = i.ReadValue<Vector2>();

            PlayerInput.PlayerActions.Walk.performed += i => WalkInput = true;
            PlayerInput.PlayerActions.Walk.canceled += i => WalkInput = false;

            PlayerInput.PlayerActions.Sprint.performed += i => SprintInput = true;
            PlayerInput.PlayerActions.Sprint.canceled += i => SprintInput = false;

            PlayerInput.PlayerActions.Slide.performed += i => CrouchInput = true;
            PlayerInput.PlayerActions.Slide.canceled += i => CrouchInput = false;

            PlayerInput.PlayerActions.Jump.performed += i => JumpInput = true;

            PlayerInput.PlayerActions.LightAttack.performed += i => {
                if (PlayerUIManager.instance.GetUIStackSize() <= 1)
                    LightAttackInput = true;
            };
            PlayerInput.PlayerActions.HeavyAttack.performed += i => {
                if (PlayerUIManager.instance.GetUIStackSize() <= 1)
                    HeavyAttackInput = true;
            };

            PlayerInput.PlayerActions.RightWeaponChange.performed += i => RightWeaponChangeInput = true;
            PlayerInput.PlayerActions.LeftWeaponChange.performed += i => LeftWeaponChangeInput = true;

            PlayerInput.PlayerActions.MenuSelection.performed += i => MenuSelectionInput = true;

            PlayerInput.PlayerActions.Interact.performed += i => InteractionInput = true;

            PlayerInput.PlayerActions.UIAction1.performed += i => UIAction1 = true;
            PlayerInput.PlayerActions.UIAction1.canceled += i => UIAction1 = false;

            PlayerInput.Enable();
        }

        private void OnDisable() {
            PlayerInput.Disable();
        }
    }
}