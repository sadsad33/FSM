using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterCombatStanceState : AICharacterGroundedState {
        BehaviourTree combatStanceBT;
        BTNode.Status result;
        public CharacterBehaviourCode AIBehaviourStatus { get; set; }
        public float StrafeBehaviourTimer { get; set; }

        public override void Enter(CharacterManager character) {
            base.Enter(character);

            StrafeBehaviourTimer = 2f;
            aiCharacter.isCombatStance = true;
            aiCharacter.agent.enabled = false;
            AIBehaviourStatus = CharacterBehaviourCode.Strafe;

            combatStanceBT = new("AICombatStance");

            RandomSelectorNode combatBehaviourSelector = new("AICombatBehaviourSelector");
            combatStanceBT.AddChild(combatBehaviourSelector);

            UntilSuccessNode moveStrafe = new("MoveUntilSuccess", 50);
            combatBehaviourSelector.AddChild(moveStrafe);

            moveStrafe.AddChild(new BTLeaf("DoStrafe", new StrafeStrategy(aiCharacter.transform, aiCharacter.aiEyesManager.currentTarget.transform, CharacterBehaviourCode.Strafe)));

            RandomSelectorNode attackBehaviourSelector = new("AIAttackBehaviourSelector");
            combatBehaviourSelector.AddChild(attackBehaviourSelector);
            //bool DistanceChecking() {
            //    float distance = Vector3.Distance(aiCharacter.transform.position, aiCharacter.aiEyesManager.currentTarget.transform.position);
            //    return distance <= aiCharacter.aiStatsManager.CombatStanceDistance && distance > aiCharacter.aiStatsManager.AttackDistance;
            //}
            //CheckConditionNode distanceCheck = new("DistanceCheck", DistanceChecking);
            attackBehaviourSelector.AddChild(new BTLeaf("LightAttack", new AttackStrategy(aiCharacter.transform, aiCharacter.aiEyesManager.currentTarget.transform, "OH_Sword_Attack1", CharacterBehaviourCode.LightAttack), 75));
            //attackBehaviourSelector.AddChild(new BTLeaf("HeavyAttack", new AttackStrategy(aiCharacter.transform, aiCharacter.aiEyesManager.currentTarget.transform, "OH_Sword_Attack1", CharacterBehaviourCode.HeavyAttack), 35));
            //attackBehaviourSelector.AddChild(distanceCheck);
            //distanceCheck.AddChild(new BTLeaf("RunningAttack", new AttackStrategy(aiCharacter.transform, aiCharacter.aiEyesManager.currentTarget.transform, "Running Attack", CharacterBehaviourCode.RunningAttack), 20));
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            result = combatStanceBT.Process();
            //Debug.Log(StrafeBehaviourTimer);
            if (result == BTNode.Status.Success) {
                combatStanceBT.Reset();
                //Debug.Log("트리 리셋");
            }
            //Debug.Log(AIBehaviourStatus);
            if ( AIBehaviourStatus == CharacterBehaviourCode.LightAttack) {
                Debug.Log("Transition To LightAttackState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLightAttackState);
            }
        }
    }
}