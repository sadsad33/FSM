using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionState : IState
{
    protected PlayerManager player;
    public virtual void Enter(CharacterManager character) {
        player = character as PlayerManager;
        Debug.Log("Player Current Action State : " + GetType());
    }

    public virtual void Stay(CharacterManager character) {
        if (player.isPerformingAction) return;
        HandleInput();
    }

    public virtual void Exit(CharacterManager character) {
    
    }

    public virtual void HandleInput() {
        float delta = Time.deltaTime;
    }
}
