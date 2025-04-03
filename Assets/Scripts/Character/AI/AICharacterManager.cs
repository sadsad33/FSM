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
        bool isFollowingPath = false;
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
            if (currentWaypoint != null) Debug.Log("현재 웨이포인트 : " + currentWaypoint);
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
            if (distance > aiStatsManager.attackDistance)
                PathRequestManager3D.RequestPath(transform.position, aiEyesManager.currentTarget.transform.position, OnPathFound);
            else
                Debug.Log("공격!!!");
        }


        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                Debug.Log("경로 길이 : " + path.Length);
                StopCoroutine(nameof(FollowPath));
                StartCoroutine(nameof(FollowPath));
            }
        }

        IEnumerator FollowPath() {
            currentWaypoint = path[0];
            while (true) {
                if (transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                        yield break;
                    currentWaypoint = path[targetIndex];
                }
                //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 3f * Time.deltaTime);
                yield return null;
            }
        }
    }
}
