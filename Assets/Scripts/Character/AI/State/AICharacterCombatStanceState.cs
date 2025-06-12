using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterCombatStanceState : AICharacterGroundedState {
        BehaviourTree combatStanceBT;
        bool treeHasBuilt = false;
        BTNode.Status result;
        public Enums.CharacterBehaviourCode AIBehaviourStatus { get; set; }
        public float StrafeBehaviourTimer { get; set; }

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            StrafeBehaviourTimer = 2f;
            aiCharacter.isCombatStance = true;
            aiCharacter.agent.enabled = false;
            AIBehaviourStatus = Enums.CharacterBehaviourCode.Strafe;

            if (!treeHasBuilt) {
                combatStanceBT = BuildBehaviourTree();
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
            result = combatStanceBT.Process();
            //Debug.Log(StrafeBehaviourTimer);
            if (result == BTNode.Status.Success || result == BTNode.Status.Failure) {
                combatStanceBT.Reset();
                //Debug.Log("트리 리셋");
            }

            // 공격 상태로의 전이는, 상태 내에서 이후 행동을 결정하기 위해 실행되자마자 전이되도록 함
            if (AIBehaviourStatus == Enums.CharacterBehaviourCode.LightAttack) {
                //Debug.Log("Transition To LightAttackState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLightAttackState);
            } else if (AIBehaviourStatus == Enums.CharacterBehaviourCode.HeavyAttack) {
                //Debug.Log("Transition To HeavyAttackState");
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiHeavyAttackState);
            }
        }

        private BehaviourTree BuildBehaviourTree() {
            //Debug.Log("CombatStance 트리 빌드");
            BehaviourTree tree = new("AICombatStance");

            RandomSelectorNode combatBehaviourSelector = new("AICombatBehaviourSelector");
            tree.AddChild(combatBehaviourSelector);

            bool CanDoBehaviour() {
                return !aiCharacter.isPerformingAction;
            }

            bool DistanceCheck() {
                return Vector3.Distance(aiCharacter.transform.position, aiCharacter.aiEyesManager.currentTarget.transform.position) <= aiCharacter.aiStatsManager.AttackDistance;
            }

            ConditionNode canDoStrafe = new("CanDoStrafeNow?", CanDoBehaviour);
            combatBehaviourSelector.AddChild(canDoStrafe);
            UntilSuccessNode moveStrafe = new("MoveUntilSuccess", 50);
            canDoStrafe.AddChild(moveStrafe);
            moveStrafe.AddChild(new BTLeaf("DoStrafe",
                new StrafeStrategy(aiCharacter.transform,
                aiCharacter.aiEyesManager.currentTarget.transform,
                Enums.CharacterBehaviourCode.Strafe)));

            ConditionNode canDoDodge = new("CanDoDodgeNow?", CanDoBehaviour);
            combatBehaviourSelector.AddChild(canDoDodge);
            ConditionNode distanceCheck = new("IsEnemyInRange?", DistanceCheck);
            canDoDodge.AddChild(distanceCheck);
            ConditionNode dodgeCondition = new("IsEnemyAttacking?", () => { return aiCharacter.aiEyesManager.currentTarget.isAttacking; });
            distanceCheck.AddChild(dodgeCondition);
            dodgeCondition.AddChild(new BTLeaf("Dodge", new ActionStrategy(() => {
                aiCharacter.isPerformingAction = true;
                aiCharacter.isInvulnerable = true;
                aiCharacter.cc.enabled = true;
                aiCharacter.aiAnimatorManager.PlayAnimation("OneHand_Up_Jump_B", aiCharacter.isPerformingAction);
            }), 15));

            RandomSelectorNode attackBehaviourSelector = new("AIAttackBehaviourSelector");
            combatBehaviourSelector.AddChild(attackBehaviourSelector);

            ConditionNode canDoLightAttack = new("CanDoLightAttackNow?", CanDoBehaviour);
            ConditionNode targetInLightAttackRange = new("IsTargetInLightAttackRange?", DistanceCheck);
            attackBehaviourSelector.AddChild(canDoLightAttack);
            canDoLightAttack.AddChild(targetInLightAttackRange);
            targetInLightAttackRange.AddChild(new BTLeaf("LightAttack", new ActionStrategy(() => {
                aiCharacter.isPerformingAction = true;
                AIBehaviourStatus = Enums.CharacterBehaviourCode.LightAttack;
                aiCharacter.aiAnimatorManager.PlayAnimation("OH_Sword_Attack1", aiCharacter.isPerformingAction);
            }), 75));

            ConditionNode canDoHeavyAttack = new("CanDoHeavyAttackNow?", CanDoBehaviour);
            ConditionNode targetInHeavyAttackRange = new("IsTargetInHeavyAttackRange?", DistanceCheck);
            attackBehaviourSelector.AddChild(canDoHeavyAttack);
            canDoHeavyAttack.AddChild(targetInHeavyAttackRange);
            targetInHeavyAttackRange.AddChild(new BTLeaf("HeavyAttack", new ActionStrategy(() => {
                aiCharacter.isPerformingAction = true;
                AIBehaviourStatus = Enums.CharacterBehaviourCode.HeavyAttack;
                aiCharacter.aiAnimatorManager.PlayAnimation("OH_Sword_HeavyAttack1", aiCharacter.isPerformingAction);
            }), 35));

            ConditionNode canDoRunningAttack = new("CanDoRuninngAttackNow?", CanDoBehaviour);
            ConditionNode fowardDirectionCheck = new("IsTargetInForwardDirection?",
                () => {
                    Vector3 targetDirection = aiCharacter.aiEyesManager.currentTarget.transform.position - aiCharacter.transform.position;
                    float angleWithForward = Vector3.Angle(aiCharacter.transform.forward, targetDirection);
                    return angleWithForward < 60;
                });

            ConditionNode targetInRunningAttackRange = new("IsTargetInRunningAttackRange?",
                () => {
                    float distance = Vector3.Distance(aiCharacter.transform.position, aiCharacter.aiEyesManager.currentTarget.transform.position);
                    return (distance > aiCharacter.aiStatsManager.AttackDistance &&
                    distance <= aiCharacter.aiStatsManager.CombatStanceDistance);
                });
            attackBehaviourSelector.AddChild(canDoRunningAttack);
            canDoRunningAttack.AddChild(fowardDirectionCheck);
            fowardDirectionCheck.AddChild(targetInRunningAttackRange);
            targetInRunningAttackRange.AddChild(new BTLeaf("RunningAttack", new ActionStrategy(() => {
                aiCharacter.isPerformingAction = true;
                AIBehaviourStatus = Enums.CharacterBehaviourCode.RunningAttack;
                aiCharacter.aiAnimatorManager.PlayAnimation("Running Attack", aiCharacter.isPerformingAction);
            }), 35));

            return tree;
        }
    }
}