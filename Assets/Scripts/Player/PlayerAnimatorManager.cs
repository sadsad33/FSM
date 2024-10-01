using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    public PlayerManager player;
    protected override void Awake() {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public override void PlayAnimation(string animation, bool isInteracting) {
        base.PlayAnimation(animation, isInteracting);
    }

    public void StartAction() {
        player.isPerformingAction = true;
    }

    public void FinishAction() {
        player.isPerformingAction = false;
    }
}
