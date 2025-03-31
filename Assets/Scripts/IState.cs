using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public interface IState {
        public void Enter(CharacterManager character);

        public void Stay(CharacterManager character);

        public void Exit(CharacterManager character);
    }
}