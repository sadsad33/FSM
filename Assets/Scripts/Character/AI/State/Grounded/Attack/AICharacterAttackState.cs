using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterAttackState : AICharacterGroundedState {
        protected BTNode.Status result;
        public bool IsDoingComboAttack { get; set; }
        public bool HasDone { get; set; }
        public CharacterBehaviourCode AIBehaviourStatus { get; set; }
    }
}