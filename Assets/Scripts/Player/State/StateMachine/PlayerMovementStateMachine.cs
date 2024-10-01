using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine {
    //PlayerMovementState currentPlayerMovementState;
    public PlayerIdlingState idlingState;

    #region MovingStates
    public PlayerWalkingState walkingState;
    public PlayerRunningState runningState;
    public PlayerSprintingState sprintingState;
    public PlayerRollingState rollingState;
    #endregion

    #region StoppingStates
    public PlayerLightStoppingState lightStoppingState;
    public PlayerMediumStoppingState mediumStoppingState;
    #endregion

    public PlayerFallingState fallingState;

    public PlayerLightLandingState lightLandingState;
    public PlayerMediumLandingState mediumLandingState;
    public PlayerHardLandingState hardLandingState;

    //public PlayerJumpingState jumpingState;

    public PlayerMovementStateMachine(PlayerManager player) : base(player){
        idlingState = new PlayerIdlingState();

        walkingState = new PlayerWalkingState();
        runningState = new PlayerRunningState();
        sprintingState = new PlayerSprintingState();
        rollingState = new PlayerRollingState();

        lightStoppingState = new PlayerLightStoppingState();
        mediumStoppingState = new PlayerMediumStoppingState();

        fallingState = new PlayerFallingState();
        lightLandingState = new PlayerLightLandingState();
        mediumLandingState = new PlayerMediumLandingState();
        hardLandingState = new PlayerHardLandingState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
        //currentPlayerMovementState = currentState as PlayerMovementState;
    }
}
