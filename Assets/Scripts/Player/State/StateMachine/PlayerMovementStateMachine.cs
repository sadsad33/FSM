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
    public PlayerLandToMoveState landToMoveState;
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

    #region AirborneStates
    public PlayerFallingState fallingState;
    public PlayerRunningJumpState runningJumpState;
    public PlayerStandingJumpState standingJumpState;
    #endregion

    public PlayerMovementStateMachine(PlayerManager player) : base(player){
        idlingState = new PlayerIdlingState();

        walkingState = new PlayerWalkingState();
        runningState = new PlayerRunningState();
        sprintingState = new PlayerSprintingState();
        rollingState = new PlayerRollingState();
        landToMoveState = new PlayerLandToMoveState();

        lightStoppingState = new PlayerLightStoppingState();
        mediumStoppingState = new PlayerMediumStoppingState();

        fallingState = new PlayerFallingState();
        runningJumpState = new PlayerRunningJumpState();
        standingJumpState = new PlayerStandingJumpState();
        lightLandingState = new PlayerLightLandingState();
        mediumLandingState = new PlayerMediumLandingState();
        hardLandingState = new PlayerHardLandingState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
        //currentPlayerMovementState = currentState as PlayerMovementState;
    }
}
