using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionStateMachine : StateMachine {
        
        public PlayerNotInteractingState notInteractingState;
        public PlayerLootingInteractionState lootingInteractionState;
        public PlayerLadderBottomStartInteractionState ladderBottomStartInteractionState;
        public PlayerLadderTopStartInteractionState ladderTopStartInteractionState;
        public PlayerMoveToInteractingPositionState moveToInteractingPositionState;
        public PlayerLadderTopEndInteractionState ladderTopEndInteractionState;
        public PlayerLadderBottomEndInteractionState ladderBottomEndInteractionState;

        public PlayerInteractionStateMachine(PlayerManager player) : base(player) {
            lootingInteractionState = new();
            notInteractingState = new();
            ladderBottomStartInteractionState = new();
            ladderTopStartInteractionState = new();
            moveToInteractingPositionState = new();
            ladderTopEndInteractionState = new();
            ladderBottomEndInteractionState = new();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}
