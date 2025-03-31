using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterManager : CharacterManager {
        public AICharacterStatsManager aiStatsManager;
        public AICharacterEquipmentManager aiEquipmentManager;
        public AICharacterEyesManager aiEyesManager;
        public AICharacterStateMachine acsm;

        Vector3[] path;
        int targetIndex;

        protected override void Awake() {
            base.Awake();
            aiStatsManager = GetComponent<AICharacterStatsManager>();
            aiEquipmentManager = GetComponent<AICharacterEquipmentManager>();
            aiEyesManager = GetComponent<AICharacterEyesManager>();
            acsm = new AICharacterStateMachine(this);
        }

        protected override void Start() {
            base.Start();
            acsm.ChangeState(acsm.aiIdlingState);
        }

        protected override void Update() {
            base.Update();
            CharacterInit();
            //StartMoving();
            acsm.GetCurrentState().Stay(this);
        }

        protected void LateUpdate() {
            characterAnimatorManager.animator.SetBool("isGrounded", isGrounded);
        }

        protected override void CharacterInit() {
            InAirTimer = 0f;
            YVelocity = Vector3.zero;
            GroundedYVelocity = -10f;
            GravityForce = -10f;
            FallStartYVelocity = -1.5f;
            FallingVelocitySet = false;
            GroundCheckSphereRadius = 0.3f;

            MaximumJumpHeight = 1.5f;
            JumpStartYVelocity = 2.5f;
            JumpForce = 1f;
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

        public void StartMoving() {
            if (aiEyesManager.currentTarget == null) return;
            float distance = Vector3.Distance(transform.position, aiEyesManager.currentTarget.transform.position);
            if (distance > aiStatsManager.attackDistance)
                PathRequestManager3D.RequestPath(transform.position, aiEyesManager.currentTarget.transform.position, OnPathFound);
            else
                Debug.Log("공격!!!");
        }
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath() {
            Vector3 currentWaypoint = path[0];
            while (true) {
                if (transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                        yield break;
                    currentWaypoint = path[targetIndex];
                }
                Vector3 moveDirection = currentWaypoint - transform.position;
                moveDirection.Normalize();
                Vector3 tempDirection = moveDirection;
                tempDirection.y = 0;
                moveDirection = tempDirection;
                if (moveDirection != Vector3.zero)
                    transform.forward = moveDirection;
                Debug.Log("경사 처리 전 :" + moveDirection);
                AdjustSlope(ref moveDirection);
                Debug.Log("경사 처리 후 :" + moveDirection);
                cc.Move(3f * Time.deltaTime * moveDirection);
                yield return null;
            }
        }

        private void AdjustSlope(ref Vector3 moveDirection) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f)) {
                //Debug.Log("경사면 각도 : " + Vector3.Angle(hit.normal, Vector3.up));
                //if (Vector3.Angle(hit.normal, Vector3.up) < controller.slopeLimit) {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
                //}
            }
        }
    }
}
