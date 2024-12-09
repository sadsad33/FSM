using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerActionStateMachine : StateMachine {
        public PlayerStandingActionIdlingState standingActionIdlingState;
        public PlayerAirborneActionIdlingState airborneActionIdlingState;
        public PlayerSlidingActionIdlingState slidingActionIdlingState;
        public PlayerCrouchedActionIdlingState crouchedActionIdlingState;
        public PlayerSprintingActionIdlingState sprintingActionIdlingState;

        public PlayerOneHandSwordFirstAttackState oneHandSwordFirstAttackState;
        public PlayerOneHandSwordComboAttackState oneHandSwordComboAttackState;
        public PlayerOneHandSwordFinalAttackState oneHandSwordFinalAttackState;
        public PlayerOneHandSwordHeavyAttackState oneHandSwordHeavyAttackState;
        public PlayerOneHandSwordHeavyAttackComboState oneHandSwordHeavyAttackComboState;

        public PlayerMeleeJumpLightAttackState meleeJumpLightAttackState;
        public PlayerLightAttackLandingState lightAttackLandingState;
        public PlayerSlideAttackState slidingAttackState;
        public PlayerCrouchingAttackState crouchingAttackState;
        public PlayerRunningAttackState runningAttackState;

        public PlayerActionStateMachine(PlayerManager player) : base(player) {
            standingActionIdlingState = new PlayerStandingActionIdlingState();
            oneHandSwordFirstAttackState = new PlayerOneHandSwordFirstAttackState();
            oneHandSwordComboAttackState = new PlayerOneHandSwordComboAttackState();
            oneHandSwordFinalAttackState = new PlayerOneHandSwordFinalAttackState();

            airborneActionIdlingState = new PlayerAirborneActionIdlingState();
            meleeJumpLightAttackState = new PlayerMeleeJumpLightAttackState();
            lightAttackLandingState = new PlayerLightAttackLandingState();
            oneHandSwordHeavyAttackState = new PlayerOneHandSwordHeavyAttackState();
            oneHandSwordHeavyAttackComboState = new PlayerOneHandSwordHeavyAttackComboState();

            slidingActionIdlingState = new PlayerSlidingActionIdlingState();
            slidingAttackState = new PlayerSlideAttackState();

            crouchedActionIdlingState = new PlayerCrouchedActionIdlingState();
            crouchingAttackState = new PlayerCrouchingAttackState();

            sprintingActionIdlingState = new PlayerSprintingActionIdlingState();
            runningAttackState = new PlayerRunningAttackState();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}