using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine {
    //PlayerMovementState currentPlayerMovementState;
    public PlayerIdlingState idlingState;
    public PlayerWalkingState walkingState;
    public PlayerRunningState runningState;
    public PlayerSprintingState sprintingState;
    public PlayerRollingState rollingState;
    public PlayerLightStoppingState lightStoppingState;
    public PlayerMediumStoppingState mediumStoppingState;
    //public PlayerFallingState fallingState;
    //public PlayerJumpingState jumpingState;

    public PlayerMovementStateMachine(PlayerManager player) : base(player){
        idlingState = new PlayerIdlingState();
        walkingState = new PlayerWalkingState();
        runningState = new PlayerRunningState();
        sprintingState = new PlayerSprintingState();
        rollingState = new PlayerRollingState();
        lightStoppingState = new PlayerLightStoppingState();
        mediumStoppingState = new PlayerMediumStoppingState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
        //currentPlayerMovementState = currentState as PlayerMovementState;
    }
}
