using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerHandStateMachine : StateMachine {

        public PlayerHandStateMachine(PlayerManager player) : base(player) {
            
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}