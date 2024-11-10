using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateMachine : StateMachine
{
    public PlayerStandingActionIdlingState standingActionIdlingState;
    public PlayerAirborneActionIdlingState airborneActionIdlingState;
    public PlayerSlidingActionIdlingState slidingActionIdlingState;
    
    public PlayerOneHandSwordFirstAttackState oneHandSwordFirstAttackState;
    public PlayerOneHandSwordComboAttackState oneHandSwordComboAttackState;
    public PlayerOneHandSwordFinalAttackState oneHandSwordFinalAttackState;

    public PlayerMeleeJumpLightAttackState meleeJumpLightAttackState;
    public PlayerLightAttackLandingState lightAttackLandingState;
    public PlayerSlideAttackState slidingAttackState;

    public PlayerActionStateMachine(PlayerManager player) : base(player) {
        standingActionIdlingState = new PlayerStandingActionIdlingState();
        oneHandSwordFirstAttackState = new PlayerOneHandSwordFirstAttackState();
        oneHandSwordComboAttackState = new PlayerOneHandSwordComboAttackState();
        oneHandSwordFinalAttackState = new PlayerOneHandSwordFinalAttackState();

        airborneActionIdlingState = new PlayerAirborneActionIdlingState();
        meleeJumpLightAttackState = new PlayerMeleeJumpLightAttackState();
        lightAttackLandingState = new PlayerLightAttackLandingState();

        slidingActionIdlingState = new PlayerSlidingActionIdlingState();
        slidingAttackState = new PlayerSlideAttackState();
    }

    public override void ChangeState(IState newState) {
        base.ChangeState(newState);
    }
}
