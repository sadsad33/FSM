using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KBH {
    public interface IStrategy {
        // ��� Strategy�� Behaviour�� �����ؾ� �ϹǷ� BTNode�� Process �� ȣ��
        BTNode.Status Process();
        // Behavior ������ �ʱ�ȭ
        void Reset() {

        }
    }

    // ���� ���� � �ൿ�� ���� �����ϱ� ���� ����
    // ����) Ư�� ��ǥ�� ������ �̵���Ŵ
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

    // �ܼ��� Ư���� ���Ǹ��� �޾� �Ǵ��ϴ� ����
    public class ConditionStrategy : IStrategy {
        // ������ �����ϴ��� �Ǵ��� ����
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

        public StrafeStrategy(Transform entity, Transform target) {
            this.entity = entity;
            this.target = target;
            ai = entity.GetComponent<AICharacterManager>();
            aiCombatStanceState = ai.acsm.aiCombatStanceState;
            ai.cc.enabled = false;
            PickNewStrafeTarget();
        }

        public BTNode.Status Process() {
            //Debug.Log("������");
            aiCombatStanceState.StrafeBehaviourTimer -= Time.deltaTime;
            
            if (aiCombatStanceState.StrafeBehaviourTimer <= 0f) {
                //Debug.Log("�ð� �ʰ�");
                aiCombatStanceState.StrafeBehaviourTimer = 2f; 
                return BTNode.Status.Failure;
            }

            if (Vector3.Distance(ai.transform.position, currentStrafeTarget) < 0.1f) {
                //Debug.Log("�̵� �Ϸ�");
                PickNewStrafeTarget();
                //ai.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                //ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                return BTNode.Status.Success; 
            }

            //Debug.Log("���� " + ai.transform.position + " ����");
            //Debug.Log(currentStrafeTarget + "�� �̵� ��");
            //Debug.Log("�Ÿ� : " + Vector3.Distance(ai.transform.position, currentStrafeTarget));
            // �̵� ����
            Vector3 moveDirection = currentStrafeTarget - ai.transform.position;
            UpdateStrafeAnimation(moveDirection);
            entity.LookAt(target);
            entity.position = Vector3.Lerp(entity.position, currentStrafeTarget, 0.5f * Time.deltaTime);
            return BTNode.Status.Running;
        }

        void PickNewStrafeTarget() {
            //Debug.Log("������ ����");
            //strafeTimer = strafeDuration;

            float radius = ai.aiStatsManager.AttackDistance * 0.9f;
            float minAngle = -180f;
            float maxAngle = 180f;

            // AI�� ��ǥ���� �Ÿ��� �־��� ���� �� ���� ���ַ� �̵���
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
            } else if (minAngle == angleWithRight) {
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", motionSpeed, 0.1f, Time.deltaTime);
            } else if (minAngle == angleWithBackward) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", -1f * motionSpeed , 0.1f, Time.deltaTime);
            } else {
                ai.aiAnimatorManager.animator.SetFloat("Horizontal",-1f *  motionSpeed , 0.1f, Time.deltaTime);
            }
        }

        public void Reset() {
            //ai.acsm.aiCombatStanceState.StrafeBehaviourTimer = 2f;
        }
    }
    public class AttackStrategy : IStrategy {
        readonly Transform entity;
        readonly Transform target;
        readonly AICharacterManager ai;
        readonly string attackAnimation;

        bool isComboAttack;
        public AttackStrategy(Transform entity, Transform target, string attackAnimation, bool isComboAttack = false) {
            this.entity = entity;
            this.target = target;
            this.attackAnimation = attackAnimation;
            this.isComboAttack = isComboAttack;
            ai = entity.GetComponent<AICharacterManager>();
        }

        public BTNode.Status Process() {
            entity.LookAt(target);
            //Debug.Log("����");
            if (Vector3.Distance(entity.position, target.position) <= ai.aiStatsManager.AttackDistance) {
                if (!isComboAttack) {
                    if (!ai.isPerformingAction) {
                        ai.isPerformingAction = true;
                        ai.aiAnimatorManager.PlayAnimation(attackAnimation, ai.isPerformingAction);
                        return BTNode.Status.Success;
                    }
                } else if (ai.canDoComboAttack) {
                    if (!ai.acsm.aiCombatStanceState.IsDoingComboAttack) {
                        ai.acsm.aiCombatStanceState.IsDoingComboAttack = true;
                        ai.isPerformingAction = true;
                        ai.aiAnimatorManager.PlayAnimation(attackAnimation, ai.isPerformingAction);
                        return BTNode.Status.Success;
                    }
                }
                Debug.Log(attackAnimation + "���� ��");
                return BTNode.Status.Running;
            }
            Debug.Log(attackAnimation + "���� ����");
            return BTNode.Status.Failure;
        }

        // ����
    }

    // ����
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
