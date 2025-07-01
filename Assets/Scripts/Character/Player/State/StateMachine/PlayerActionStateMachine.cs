using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KBH {
    public class PlayerActionStateMachine : StateMachine {
        public PlayerStandingActionIdlingState standingActionIdlingState;
        public PlayerAirborneActionIdlingState airborneActionIdlingState;
        public PlayerSlidingActionIdlingState slidingActionIdlingState;
        public PlayerCrouchingActionIdlingState crouchedActionIdlingState;
        public PlayerSprintingActionIdlingState sprintingActionIdlingState;

        public PlayerActionStateMachine(PlayerManager player) : base(player) {
            standingActionIdlingState = new PlayerStandingActionIdlingState();
            airborneActionIdlingState = new PlayerAirborneActionIdlingState();
            slidingActionIdlingState = new PlayerSlidingActionIdlingState();
            crouchedActionIdlingState = new PlayerCrouchingActionIdlingState();
            sprintingActionIdlingState = new PlayerSprintingActionIdlingState();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}