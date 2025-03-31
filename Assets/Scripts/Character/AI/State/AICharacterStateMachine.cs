using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterStateMachine : StateMachine {

        public AICharacterIdlingState aiIdlingState;
        public AICharacterDrawingWeaponState aiDrawingWeaponState;
        public AICharacterPursuingState aiPursuingState;
        public AICharacterFallingState aiFallingState;
        public AICharacterBewaringState aiBewaringState;
        public AICharacterStateMachine(CharacterManager character) : base(character) {
            aiIdlingState = new();
            aiDrawingWeaponState = new();
            aiPursuingState = new();
            aiFallingState = new();
            aiBewaringState = new();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}
