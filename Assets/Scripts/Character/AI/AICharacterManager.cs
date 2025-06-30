using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace KBH {
    public class AICharacterManager : CharacterManager {
        public AICharacterInteractionManager aiInteractionManager;
        public AICharacterEquipmentManager aiEquipmentManager;
        public AICharacterAnimatorManager aiAnimatorManager;
        public AICharacterStatsManager aiStatsManager;
        public AICharacterEyesManager aiEyesManager;
        public AICharacterStateMachine acsm;
        public NavMeshAgent agent;

        public bool isCombatStance;
        public Vector3 TargetPosition {
            get; set;
        }

        protected override void Awake() {
            base.Awake();
            cc = GetComponent<CharacterController>();
            //Debug.Log("AI의 컨트롤러 : " + cc.GetInstanceID());
            agent = GetComponent<NavMeshAgent>();
            aiStatsManager = GetComponent<AICharacterStatsManager>();
            aiEquipmentManager = GetComponent<AICharacterEquipmentManager>();
            aiEyesManager = GetComponent<AICharacterEyesManager>();
            aiAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiInteractionManager = GetComponentInChildren<AICharacterInteractionManager>();
            CharacterInit();
            acsm = new AICharacterStateMachine(this);
        }

        protected override void Start() {
            base.Start();
            acsm.ChangeState(acsm.aiIdlingState);
        }

        protected override void Update() {
            base.Update();
            acsm.GetCurrentState().Stay(this);
        }

        protected void LateUpdate() {
            aiAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
            aiAnimatorManager.animator.SetBool("isAttacking", isAttacking);
            aiAnimatorManager.animator.SetBool("isGrounded", isGrounded);
            aiAnimatorManager.animator.SetBool("isCrouched", isCrouching);
            aiAnimatorManager.animator.SetBool("isClimbing", isClimbing);
            aiAnimatorManager.animator.SetBool("rightFootUp", rightFootUp);
            aiAnimatorManager.animator.SetBool("isCombatStance", isCombatStance);
            aiAnimatorManager.animator.SetBool("isDead", aiStatsManager.isDead);
        }

        protected override void CharacterInit() {
            base.CharacterInit();
            aiStatsManager.AttackDistance = 1.5f;
            aiStatsManager.DetectionRadius = 5f;
            aiStatsManager.CombatStanceDistance = 3f;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            //Gizmos.DrawSphere(transform.position, GroundCheckSphereRadius);
            Gizmos.DrawRay(transform.position + (Vector3.up * bottomGroundCheckRayStartingYPosition), -transform.up * bottomGroundCheckRayMaxDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.forward * groundCheckRaycastStartingPosition.x);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.forward * groundCheckRaycastStartingPosition.x);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), transform.right * groundCheckRaycastStartingPosition.x);
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckRaycastStartingPosition.y), -transform.right * groundCheckRaycastStartingPosition.x);
        }
    }
}
