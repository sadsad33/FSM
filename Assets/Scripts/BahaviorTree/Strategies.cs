using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KBH {
    public interface IStrategy {
        // 모든 Strategy은 Behaviour을 실행해야 하므로 BTNode의 Process 를 호출
        BTNode.Status Process();
        // Behavior 실행후 초기화
        void Reset() {

        }
    }

    // 조건 없이 어떤 행동을 그저 수행하기 위한 전략
    // 예시) 특정 좌표로 유닛을 이동시킴
    public class ActionStrategy : IStrategy {
        readonly Action doSomething;

        public ActionStrategy(Action doSomething) {
            this.doSomething = doSomething;
        }

        public BTNode.Status Process() {
            doSomething();
            return BTNode.Status.Success;
        }
    }

    // 단순히 특정한 조건만을 받아 판단하는 전략
    public class ConditionStrategy : IStrategy {
        // 조건을 만족하는지 판단할 수식
        readonly Func<bool> predicate;

        public ConditionStrategy(Func<bool> predicate) {
            this.predicate = predicate;
        }

        public BTNode.Status Process() => predicate() ? BTNode.Status.Success : BTNode.Status.Failure;

    }

    public class StrafeStrategy : IStrategy {
        readonly Transform entity;
        readonly Transform target;
        readonly AICharacterManager ai;

        Vector3 currentStrafeTarget;
        readonly AICharacterCombatStanceState aiCombatStanceState;

        public StrafeStrategy(Transform entity, Transform target, CharacterBehaviourCode strafeCode) {
            this.entity = entity;
            this.target = target;
            ai = entity.GetComponent<AICharacterManager>();
            aiCombatStanceState = ai.acsm.aiCombatStanceState;
            aiCombatStanceState.AIBehaviourStatus = strafeCode;
            ai.cc.enabled = false;
            PickNewStrafeTarget();
        }

        public BTNode.Status Process() {
            //Debug.Log("간보기");
            aiCombatStanceState.StrafeBehaviourTimer -= Time.deltaTime;

            if (aiCombatStanceState.StrafeBehaviourTimer <= 0f) {
                //Debug.Log("시간 초과");
                aiCombatStanceState.StrafeBehaviourTimer = 2f;
                return BTNode.Status.Failure;
            }

            if (Vector3.Distance(ai.transform.position, target.position) <= ai.aiStatsManager.AttackDistance) {
                //Debug.Log(Vector3.Distance(ai.transform.position, target.position));
                return BTNode.Status.Success;
            }

            if (Vector3.Distance(ai.transform.position, currentStrafeTarget) < 0.1f) {
                //Debug.Log("이동 완료");
                PickNewStrafeTarget();
                //ai.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                //ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                return BTNode.Status.Success;
            }

            //Debug.Log("현재 " + ai.transform.position + " 에서");
            //Debug.Log(currentStrafeTarget + "로 이동 중");
            //Debug.Log("거리 : " + Vector3.Distance(ai.transform.position, currentStrafeTarget));
            // 이동 실행
            Vector3 moveDirection = currentStrafeTarget - ai.transform.position;
            UpdateStrafeAnimation(moveDirection);
            entity.LookAt(target);
            entity.position = Vector3.Lerp(entity.position, currentStrafeTarget, 0.5f * Time.deltaTime);
            return BTNode.Status.Running;
        }

        void PickNewStrafeTarget() {
            //Debug.Log("포지션 수정");
            //strafeTimer = strafeDuration;

            float radius = ai.aiStatsManager.AttackDistance * 0.9f;
            float minAngle = -180f;
            float maxAngle = 180f;

            // AI와 목표물의 거리가 멀어질 수록 앞 방향 위주로 이동함
            if (Vector3.Distance(entity.position, target.position) >= ai.aiStatsManager.CombatStanceDistance) {
                minAngle = -90f;
                maxAngle = 90f;
            }

            float angle = UnityEngine.Random.Range(minAngle, maxAngle);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * ai.transform.forward;
            Vector3 candidatePos = ai.transform.position + direction.normalized * radius;

            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                currentStrafeTarget = hit.position;
            } else {
                currentStrafeTarget = ai.transform.position;
            }
        }

        void UpdateStrafeAnimation(Vector3 moveDirection) {
            float angleWithForward = Vector3.Angle(ai.transform.forward, moveDirection);
            float angleWithRight = Vector3.Angle(ai.transform.right, moveDirection);
            float angleWithBackward = Vector3.Angle(-ai.transform.forward, moveDirection);
            float angleWithLeft = Vector3.Angle(-ai.transform.right, moveDirection);

            float remainingDistance = Vector3.Distance(entity.position, currentStrafeTarget);
            float motionSpeed = Mathf.Clamp01(remainingDistance);
            motionSpeed *= 0.35f;
            float minAngle = Mathf.Min(angleWithForward, angleWithRight, angleWithBackward, angleWithLeft);
            if (minAngle == angleWithForward) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", motionSpeed, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

            } else if (minAngle == angleWithRight) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", motionSpeed, 0.1f, Time.deltaTime);
            } else if (minAngle == angleWithBackward) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", -1f * motionSpeed, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            } else {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", -1f * motionSpeed, 0.1f, Time.deltaTime);
            }
        }

        public void Reset() {
            //ai.acsm.aiCombatStanceState.StrafeBehaviourTimer = 2f;
        }
    }

    public class ComboAttackStrategy : IStrategy {
        readonly Transform entity;
        readonly Transform target;
        readonly AICharacterManager ai;
        readonly string attackAnimation;
        readonly CharacterBehaviourCode currentAttackCode;
        readonly AICharacterLightAttackState aiLightAttackState;

        public ComboAttackStrategy(Transform entity, Transform target, string attackAnimation, CharacterBehaviourCode attackCode) {
            this.entity = entity;
            this.target = target;
            this.attackAnimation = attackAnimation;
            currentAttackCode = attackCode;

            ai = entity.GetComponent<AICharacterManager>();
            aiLightAttackState = ai.acsm.aiLightAttackState;
        }

        public BTNode.Status Process() {
            entity.LookAt(target);
            if (Vector3.Distance(ai.transform.position, target.position) <= ai.aiStatsManager.AttackDistance) {
                if (ai.isPerformingAction) {
                    if (ai.canDoComboAttack && !aiLightAttackState.IsDoingComboAttack) {
                        Debug.Log("콤보");
                        ai.isPerformingAction = true;
                        aiLightAttackState.IsDoingComboAttack = true;
                        ai.canDoComboAttack = false;
                        ai.aiAnimatorManager.PlayAnimation(attackAnimation, ai.isPerformingAction);
                        return BTNode.Status.Running;
                    }
                }
            }

            if (!ai.isPerformingAction) {
                aiLightAttackState.IsDoingComboAttack = false;
                aiLightAttackState.HasDone = true;
                return BTNode.Status.Success;
            }
            return BTNode.Status.Failure;
        }

       
    }

    public class AttackStrategy : IStrategy {
        readonly Transform entity;
        readonly Transform target;
        readonly AICharacterManager ai;
        readonly string attackAnimation;
        readonly CharacterBehaviourCode currentAttackCode;
        readonly AICharacterCombatStanceState aiCombatStanceState;
        public AttackStrategy(Transform entity, Transform target, string attackAnimation, CharacterBehaviourCode attackCode) {
            this.entity = entity;
            this.target = target;
            this.attackAnimation = attackAnimation;
            currentAttackCode = attackCode;

            ai = entity.GetComponent<AICharacterManager>();
            aiCombatStanceState = ai.acsm.aiCombatStanceState;
        }

        public BTNode.Status Process() {
            entity.LookAt(target);
            if (Vector3.Distance(ai.transform.position, target.position) <= ai.aiStatsManager.AttackDistance) {
                if (!ai.isPerformingAction) {
                    Debug.Log("일반 공격");
                    ai.isPerformingAction = true;
                    aiCombatStanceState.AIBehaviourStatus = currentAttackCode;
                    ai.aiAnimatorManager.PlayAnimation(attackAnimation, ai.isPerformingAction);
                    return BTNode.Status.Success;
                }
                return BTNode.Status.Running;
            }

            return BTNode.Status.Failure;
        }
        // 리셋

    }

    // 예시
    public class PatrolStrategy : IStrategy {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f) {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }
        public BTNode.Status Process() {
            if (currentIndex == patrolPoints.Count) return BTNode.Status.Success;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target);

            if (isPathCalculated && agent.remainingDistance < 0.1f) {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending) {
                isPathCalculated = true;
            }

            return BTNode.Status.Running;
        }

        public void Reset() => currentIndex = 0;
    }
}
