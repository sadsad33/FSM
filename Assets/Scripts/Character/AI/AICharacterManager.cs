using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace KBH {
    public class AICharacterManager : CharacterManager {
        public AICharacterStatsManager aiStatsManager;
        public AICharacterEquipmentManager aiEquipmentManager;
        public AICharacterEyesManager aiEyesManager;
        public AICharacterStateMachine acsm;
        public AICharacterAnimatorManager aiAnimatorManager;
        public AICharacterInteractionManager aiInteractionManager;
        public NavMeshAgent agent;

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
            acsm = new AICharacterStateMachine(this);
        }

        protected override void Start() {
            base.Start();
            acsm.ChangeState(acsm.aiIdlingState);
        }

        protected override void Update() {
            base.Update();
            CharacterInit();
            acsm.GetCurrentState().Stay(this);
        }

        protected void LateUpdate() {
            aiAnimatorManager.animator.SetBool("isPerformingAction", isPerformingAction);
            aiAnimatorManager.animator.SetBool("isAttacking", isAttacking);
            aiAnimatorManager.animator.SetBool("isGrounded", isGrounded);
            aiAnimatorManager.animator.SetBool("isCrouched", isCrouched);
            aiAnimatorManager.animator.SetBool("isClimbing", isClimbing);
            aiAnimatorManager.animator.SetBool("rightFootUp", rightFootUp);
        }

        protected override void CharacterInit() {
            base.CharacterInit();
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
