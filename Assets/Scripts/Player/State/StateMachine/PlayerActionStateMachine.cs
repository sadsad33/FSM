using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateMachine : StateMachine
{
    public PlayerActionIdlingState actionIdlingState;
    public PlayerActionStateMachine(PlayerManager player) : base(player) {
        actionIdlingState = new PlayerActionIdlingState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
    }
}
