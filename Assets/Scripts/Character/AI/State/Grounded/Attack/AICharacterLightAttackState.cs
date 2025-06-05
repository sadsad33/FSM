using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLightAttackState : AICharacterAttackState {
        BehaviourTree lightAttackBT;

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
        }

        public override void Thinking() {
            base.Thinking();
            result = lightAttackBT.Process();

            // CombatStanceState로의 전이는 행동을 완전히 끝마친 후에 전이되도록 함
            if (HasDone) {
                Debug.Log("Transition To CombatStanceState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            }
        }
    }
}