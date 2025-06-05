using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterHeavyAttackState : AICharacterAttackState {
        BehaviourTree heavyAttackBT;
        
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            HasDone = false;
            IsDoingComboAttack = false;

            heavyAttackBT = new("AILightAttack");

            RandomSelectorNode comboAttackBehaviourSelector = new("AIComboAttackBehaviourSelector");
            heavyAttackBT.AddChild(comboAttackBehaviourSelector);

            comboAttackBehaviourSelector.AddChild(new BTLeaf("LightAttackCombo",
                new ComboAttackStrategy(aiCharacter.transform,
                aiCharacter.aiEyesManager.currentTarget.transform,
                "OH_Sword_Attack2",
                this)));
            comboAttackBehaviourSelector.AddChild(new BTLeaf("HeavyAttackCombo",
                new ComboAttackStrategy(aiCharacter.transform,
                aiCharacter.aiEyesManager.currentTarget.transform,
                "OH_Sword_HeavyAttack2",
                this)));
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void Thinking() {
            base.Thinking();
            result = heavyAttackBT.Process();

            // CombatStanceState로의 전이는 행동을 완전히 끝마친 후에 전이되도록 함
            if (HasDone) {
                Debug.Log("Transition To CombatStanceState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            }
        }
    }
}
