using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStateMachine : StateMachine {
        //PlayerMovementState currentPlayerMovementState;
        public PlayerIdlingState idlingState;

        #region CrouchingState
        public PlayerStandToCrouchState standToCrouchState;
        public PlayerCrouchToStandState crouchToStandState;
        public PlayerCrouchIdleState crouchIdleState;
        public PlayerCrouchWalkState crouchWalkState;
        #endregion

        #region MovingStates
        public PlayerWalkingState walkingState;
        public PlayerRunningState runningState;
        public PlayerSprintingState sprintingState;
        public PlayerRollingState rollingState;
        public PlayerLandToMoveState landToMoveState;
        public PlayerSlidingState slidingState;
        #endregion

        #region StoppingStates
        public PlayerLightStoppingState lightStoppingState;
        public PlayerMediumStoppingState mediumStoppingState;
        #endregion

        #region LandingStates
        public PlayerLightLandingState lightLandingState;
        public PlayerMediumLandingState mediumLandingState;
        public PlayerHardLandingState hardLandingState;
        #endregion

        #region ClimbingStates
        public PlayerRightFootUpIdlingState rightFootUpIdlingState;
        public PlayerLeftFootUpIdlingState leftFootUpIdlingState;
        public PlayerClimbingUpState climbingUpState;
        public PlayerClimbingDownState climbingDownState;
        #endregion

        #region AirborneStates
        public PlayerFallingState fallingState;
        public PlayerRunningJumpState runningJumpState;
        public PlayerStandingJumpState standingJumpState;
        #endregion

        public PlayerOneHandWeaponLightAttackState oneHandWeaponLightAttackState;
        public PlayerOneHandWeaponLightComboAttackState oneHandWeaponLightComboAttackState;
        public PlayerOneHandWeaponLightFinalAttackState oneHandWeaponLightFinalAttackState;

        public PlayerOneHandWeaponHeavyAttackState oneHandWeaponHeavyAttackState;
        public PlayerOneHandWeaponHeavyComboAttackState oneHandWeaponHeavyAttackComboState;

        public PlayerMeleeJumpLightAttackState meleeJumpLightAttackState;
        public PlayerLightAttackLandingState lightAttackLandingState;

        public PlayerSlideAttackState slidingAttackState;
        public PlayerRunningAttackState runningAttackState;
        public PlayerCrouchingAttackState crouchingAttackState;

        public PlayerWeaponArtActionState weaponArtActionState;
        public PlayerHasBeenParriedState hasBeenParriedState;

        public PlayerRipostingState ripostingState;
        public PlayerBackstabbingState backstabbingState;

        public PlayerGroundedHitState groundedHitState;

        public PlayerStateMachine(PlayerManager player) : base(player) {

            idlingState = new PlayerIdlingState();
            
            standToCrouchState = new PlayerStandToCrouchState();
            crouchToStandState = new PlayerCrouchToStandState();
            crouchIdleState = new PlayerCrouchIdleState();
            crouchWalkState = new PlayerCrouchWalkState();

            walkingState = new PlayerWalkingState();
            runningState = new PlayerRunningState();
            sprintingState = new PlayerSprintingState();
            rollingState = new PlayerRollingState();
            landToMoveState = new PlayerLandToMoveState();
            slidingState = new PlayerSlidingState();

            lightStoppingState = new PlayerLightStoppingState();
            mediumStoppingState = new PlayerMediumStoppingState();

            fallingState = new PlayerFallingState();
            runningJumpState = new PlayerRunningJumpState();
            standingJumpState = new PlayerStandingJumpState();
            lightLandingState = new PlayerLightLandingState();
            mediumLandingState = new PlayerMediumLandingState();
            hardLandingState = new PlayerHardLandingState();

            rightFootUpIdlingState = new();
            leftFootUpIdlingState = new();
            climbingUpState = new();
            climbingDownState = new();

            oneHandWeaponLightAttackState = new();
            oneHandWeaponLightComboAttackState = new();
            oneHandWeaponLightFinalAttackState = new();

            oneHandWeaponHeavyAttackState = new();
            oneHandWeaponHeavyAttackComboState = new();

            meleeJumpLightAttackState = new();
            lightAttackLandingState = new();

            slidingAttackState = new();
            runningAttackState = new();
            crouchingAttackState = new();
            weaponArtActionState = new();

            hasBeenParriedState = new();
            ripostingState = new();
            backstabbingState = new();

            groundedHitState = new();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
            //currentPlayerMovementState = currentState as PlayerMovementState;
        }
    }
}