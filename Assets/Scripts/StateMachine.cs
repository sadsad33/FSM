using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class StateMachine {
        protected IState currentState;
        public CharacterManager Character { get; }

        public StateMachine(CharacterManager character) {
            Character = character;
        }

        public virtual void ChangeState(IState newState) {
            currentState?.Exit(Character);
            currentState = newState;
            currentState.Enter(Character);
        }

        public IState GetCurrentState() {
            return currentState;
        }

        public virtual void StopStateMachine() {
            //TODO
            //상태머신 일시중지
            //일시 중지 시점에서의 상태를 기억
        }

        public virtual void ResumeStateMachine() {
            //일시 중지된 시점의 상태를 다시 재개
        }
    }
}