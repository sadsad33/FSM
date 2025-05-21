using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterCombatStanceState : AICharacterGroundedState {
        BehaviourTree combatStanceBT;
        BTNode.Status result;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isCombatStance = true;
            aiCharacter.agent.enabled = false;

            combatStanceBT = new("AICombatStance");
            SelectorNode combatBehaviourSelector = new("AICombatBehaviourSelector");
            SelectorNode attackSelector = new("AIAttackBehaviourSelector");
            //combatBehaviourSelector.AddChild(attackSelector);

            #region AttackBehaviour
            attackSelector.AddChild(new BTLeaf("LightAttack", new AttackStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform,
                aiCharacter.aiStatsManager.AttackDistance)));
            attackSelector.AddChild(new BTLeaf("LightAttackCombo", new AttackStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform,
                aiCharacter.aiStatsManager.AttackDistance)));
            attackSelector.AddChild(new BTLeaf("HeavyAttack", new AttackStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform,
                aiCharacter.aiStatsManager.AttackDistance)));
            attackSelector.AddChild(new BTLeaf("HeavyAttackCombo", new AttackStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform,
                aiCharacter.aiStatsManager.AttackDistance)));
            attackSelector.AddChild(new BTLeaf("RunningAttack", new AttackStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform,
                aiCharacter.aiStatsManager.AttackDistance)));
            #endregion
            UntilSuccessNode moveUntilSuccess = new("MoveUntilSuccess");
            moveUntilSuccess.AddChild(new BTLeaf("Strafe", new StrafeStrategy(aiCharacter.transform,
                aiCharacter.agent,
                aiCharacter.aiEyesManager.currentTarget.transform)));

            combatBehaviourSelector.AddChild(moveUntilSuccess);
            
            // 회피

            // 방어

            // 패리
            
            // 스킬

            combatStanceBT.AddChild(combatBehaviourSelector);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            result = combatStanceBT.Process();
            if (result == BTNode.Status.Success) Debug.Log("성공!");
        }
    }
}