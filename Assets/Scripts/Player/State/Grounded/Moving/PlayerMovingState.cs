using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        //Debug.Log("Moving State Stay ÀÇ MoveDirection : " + moveDirection);
    }

    public override void Exit(CharacterManager character) {
    
    }

    public override void HandleInput() {
        base.HandleInput();
        //Debug.Log("Moving State HandleInput ÀÇ MoveDirection : " + moveDirection);
    }
}
