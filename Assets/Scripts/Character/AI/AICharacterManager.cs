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
        public Vector3 currentWaypoint;

        protected override void Awake() {
            base.Awake();
            currentWaypoint = transform.position;
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
            //if (currentWaypoint != null) Debug.Log("현재 웨이포인트 : " + currentWaypoint);
            acsm.GetCurrentState().Stay(this);
        }

        protected void LateUpdate() {
            characterAnimatorManager.animator.SetBool("isGrounded", isGrounded);
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

        public void StartMoving() {
            if (aiEyesManager.currentTarget == null) return;
            float distance = Vector3.Distance(transform.position, aiEyesManager.currentTarget.transform.position);
            if (distance > aiStatsManager.attackDistance) {
                //if (WaypointProgressingStatus()) {
                Debug.Log("경로 요청");
                PathRequestManager3D.RequestPath(transform.position, aiEyesManager.currentTarget.transform.position, OnPathFound);
                //}
            } else
                Debug.Log("공격!!!");
        }

        bool WaypointProgressingStatus() {
            return currentWaypoint == transform.position;
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                //Debug.Log("경로 길이 : " + path.Length);
                StopCoroutine(nameof(FollowPath));
                StartCoroutine(nameof(FollowPath));
            }
        }

        IEnumerator FollowPath() {
            currentWaypoint = path[0];
            Debug.Log("경로 따라가기");
            while (true) {
                //if (transform.position == currentWaypoint) {
                if(Vector3.Distance(transform.position, currentWaypoint) < 0.5f){
                    targetIndex++;
                    if (targetIndex >= path.Length)
                        yield break;
                    currentWaypoint = path[targetIndex];
                }
                Vector3 moveDirection = currentWaypoint - transform.position;
                moveDirection.y = 0;
                moveDirection.Normalize();
                Vector3 temp = moveDirection;
                temp.y = 0;
                if (temp != Vector3.zero) transform.forward = temp;
                cc.Move(3f * Time.deltaTime * moveDirection);
                //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 3f * Time.deltaTime);
                yield return null;
            }
        }
    }
}
