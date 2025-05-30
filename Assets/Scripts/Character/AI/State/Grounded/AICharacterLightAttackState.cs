using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLightAttackState : AICharacterGroundedState {
        BehaviourTree lightAttackBT;
        BTNode.Status result;
        public bool IsDoingComboAttack { get; set; }
        public bool HasDone { get; set; }
        public CharacterBehaviourCode AIBehaviourStatus { get; set; }

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            HasDone = false;
            IsDoingComboAttack = false;

            lightAttackBT = new("AILightAttack");

            RandomSelectorNode comboAttackBehaviourSelector = new("AIComboAttackBehaviourSelector");
            lightAttackBT.AddChild(comboAttackBehaviourSelector);

            comboAttackBehaviourSelector.AddChild(new BTLeaf("LightAttackCombo",
                new ComboAttackStrategy(aiCharacter.transform,
                aiCharacter.aiEyesManager.currentTarget.transform,
                "OH_Sword_Attack2",
                CharacterBehaviourCode.ComboAttack)));
            comboAttackBehaviourSelector.AddChild(new BTLeaf("HeavyAttackCombo",
                new ComboAttackStrategy(aiCharacter.transform,
                aiCharacter.aiEyesManager.currentTarget.transform,
                "OH_Sword_HeavyAttack2",
                CharacterBehaviourCode.ComboAttack)));
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            result = lightAttackBT.Process();
            if (HasDone) {
                Debug.Log("Transition To CombatStanceState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            }
        }
    }
}