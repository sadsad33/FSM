using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateMachine : StateMachine
{
    public PlayerActionIdlingState actionIdlingState;
    public PlayerOneHandSwordFirstAttackState oneHandSwordFirstAttackState;
    public PlayerActionStateMachine(PlayerManager player) : base(player) {
        actionIdlingState = new PlayerActionIdlingState();
        oneHandSwordFirstAttackState = new PlayerOneHandSwordFirstAttackState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
    }
}
