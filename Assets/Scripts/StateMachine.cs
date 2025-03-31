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
            //���¸ӽ� �Ͻ�����
            //�Ͻ� ���� ���������� ���¸� ���
        }

        public virtual void ResumeStateMachine() {
            //�Ͻ� ������ ������ ���¸� �ٽ� �簳
        }
    }
}