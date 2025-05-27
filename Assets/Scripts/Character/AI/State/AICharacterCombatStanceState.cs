using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterCombatStanceState : AICharacterGroundedState {
        BehaviourTree combatStanceBT;
        BTNode.Status result;
        public enum Behaviour { Strafe, LightAttack, HeavyAttack, RunningAttack, Parry, Defend };
        Behaviour currentBehaviour;

        public float StrafeBehaviourTimer { get; set; }

        public bool IsDoingComboAttack { get; set; }

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            IsDoingComboAttack = false;

            StrafeBehaviourTimer = 2f;
            aiCharacter.isCombatStance = true;
            aiCharacter.agent.enabled = false;

            combatStanceBT = new("AICombatStance");

            PrioritySelectorNode combatBehaviourSelector = new("AICombatBehaviourSelector");
            combatStanceBT.AddChild(combatBehaviourSelector);
            
            UntilSuccessNode moveStrafe = new("MoveUntilSuccess", 50);
            combatBehaviourSelector.AddChild(moveStrafe);

            moveStrafe.AddChild(new BTLeaf("DoStrafe", new StrafeStrategy(aiCharacter.transform, aiCharacter.aiEyesManager.currentTarget.transform)));

            // 회피

            // 방어

            // 패리

            // 스킬

        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            result = combatStanceBT.Process();
            IsDoingComboAttack = false;
            Debug.Log(StrafeBehaviourTimer);
            if (result == BTNode.Status.Success) {
                combatStanceBT.Reset();
                //Debug.Log("트리 리셋");
            }
        }
    }
}