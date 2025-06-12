using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterLightAttackState : AICharacterAttackState {
        BehaviourTree lightAttackBT;
        bool treeHasBuilt = false;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //HasDone = false;
            IsDoingComboAttack = false;

            if (!treeHasBuilt) {
                lightAttackBT = BuildBehaviourTree();
                treeHasBuilt = true;
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            result = lightAttackBT.Process();
            lightAttackBT.Reset();
            // CombatStanceState로의 전이는 행동을 완전히 끝마친 후에 전이되도록 함
            if (!aiCharacter.isPerformingAction) {
                IsDoingComboAttack = false;
                //Debug.Log("Transition To CombatStanceState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiCombatStanceState);
            }
        }

        private BehaviourTree BuildBehaviourTree() {
            //Debug.Log("LightAttack 트리 빌드");
            BehaviourTree tree = new("AILightAttack");

            RandomSelectorNode comboAttackBehaviourSelector = new("AIComboAttackBehaviourSelector");
            tree.AddChild(comboAttackBehaviourSelector);

            bool DistanceCheck() {
                return Vector3.Distance(aiCharacter.transform.position, aiCharacter.aiEyesManager.currentTarget.transform.position) <= aiCharacter.aiStatsManager.AttackDistance;
            }

            bool ComboConditionCheck() {
                return (aiCharacter.canDoComboAttack && !IsDoingComboAttack);
            }

            ConditionNode lightComboAttackDistanceCheck = new("IsTargetInLightComboAttackRange?", DistanceCheck);
            comboAttackBehaviourSelector.AddChild(lightComboAttackDistanceCheck);
            ConditionNode lightComboAttackConditionCheck = new("CanDoComboLightComboAttackNow?", ComboConditionCheck);
            lightComboAttackDistanceCheck.AddChild(lightComboAttackConditionCheck);
            lightComboAttackConditionCheck.AddChild(new BTLeaf("LightComboAttack", new ActionStrategy(() => {
                //Debug.Log("약공 약공 콤보");
                aiCharacter.isPerformingAction = true;
                IsDoingComboAttack = true;
                aiCharacter.canDoComboAttack = false;
                AIBehaviourStatus = Enums.CharacterBehaviourCode.ComboAttack;
                aiCharacter.aiAnimatorManager.PlayAnimation("OH_Sword_Attack2", aiCharacter.isPerformingAction);
            })));

            ConditionNode heavyComboAttackDistanceCheck = new("IsTargetInHeavyComboAttackRange?", DistanceCheck);
            comboAttackBehaviourSelector.AddChild(heavyComboAttackDistanceCheck);
            ConditionNode heavyComboAttackConditionCheck = new("CanDoComboHeavyComboAttackNow?", ComboConditionCheck);
            heavyComboAttackDistanceCheck.AddChild(heavyComboAttackConditionCheck);
            heavyComboAttackConditionCheck.AddChild(new BTLeaf("HeavyComboAttack", new ActionStrategy(() => {
                //Debug.Log("약공 강공 콤보");
                aiCharacter.isPerformingAction = true;
                IsDoingComboAttack = true;
                aiCharacter.canDoComboAttack = false;
                AIBehaviourStatus = Enums.CharacterBehaviourCode.ComboAttack;
                aiCharacter.aiAnimatorManager.PlayAnimation("OH_Sword_HeavyAttack2", aiCharacter.isPerformingAction);
            })));

            return tree;
        }
    }
}