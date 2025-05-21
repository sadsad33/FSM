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
        readonly NavMeshAgent agent;
        readonly Transform target;
        readonly float strafeDuration = 1.5f; // 1.5�� ���� ������

        readonly AICharacterManager ai;

        Vector3 currentStrafeTarget;
        float strafeTimer;

        public StrafeStrategy(Transform entity, NavMeshAgent agent, Transform target) {
            this.entity = entity;
            this.agent = agent;
            this.target = target;

            ai = entity.GetComponent<AICharacterManager>();
            ai.agent.enabled = true;

            PickNewStrafeTarget();
        }

        public BTNode.Status Process() {
            strafeTimer -= Time.deltaTime;

            if (Vector3.Distance(ai.transform.position, currentStrafeTarget) < 0.1f || strafeTimer <= 0f) {
                PickNewStrafeTarget(); // ���� �ð� or ���� �� �� ��ǥ ����
            }

            Vector3 moveDirection = currentStrafeTarget - ai.transform.position;
            ai.agent.SetDestination(currentStrafeTarget);
            entity.LookAt(target);
            if (ai.cc.enabled) ai.cc.Move(0.5f * Time.deltaTime * moveDirection);

            UpdateStrafeAnimation(moveDirection);

            return BTNode.Status.Running;
        }

        void PickNewStrafeTarget() {
            strafeTimer = strafeDuration;
            float vertical = ai.aiAnimatorManager.animator.GetFloat("Vertical");
            float horizontal = ai.aiAnimatorManager.animator.GetFloat("Horizontal");
            ai.aiAnimatorManager.animator.SetFloat("Vertical", vertical * 0.5f, 0.1f, Time.deltaTime);
            ai.aiAnimatorManager.animator.SetFloat("Horizontal", horizontal * 0.5f, 0.1f, Time.deltaTime);
            float radius = ai.aiStatsManager.AttackDistance * 0.9f;
            float angle = UnityEngine.Random.Range(-90f, 90f);
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

            float minAngle = Mathf.Min(angleWithForward, angleWithRight, angleWithBackward, angleWithLeft);

            if (minAngle == angleWithForward) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
            } else if (minAngle == angleWithRight) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0.5f, 0.1f, Time.deltaTime);
            } else if (minAngle == angleWithBackward) {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", -0.5f, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
            } else {
                ai.aiAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                ai.aiAnimatorManager.animator.SetFloat("Horizontal", -0.5f, 0.1f, Time.deltaTime);
            }
        }

        public void Reset() {
            ai.agent.enabled = false;
        }
    }

    public class AttackStrategy : IStrategy {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly Transform target;
        readonly float attackDistance;

        public AttackStrategy(Transform entity, NavMeshAgent agent, Transform target, float attackDistance) {
            this.entity = entity;
            this.agent = agent;
            this.target = target;
            this.attackDistance = attackDistance;
        }

        public BTNode.Status Process() {
            float distance = Vector3.Distance(entity.position, target.position);
            return BTNode.Status.Running;
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
