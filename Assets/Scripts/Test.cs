using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public CharacterController cc;
    public Transform target;
    public float moveSpeed = 5f;   // 이동 속도
    public float slerpSpeed = 2f;  // 보간 속도
    private void Awake() {
        cc = GetComponent<CharacterController>();
    }

    private void Update() {
        if (target != null) {
            // 현재 위치에서 목표 위치로 향하는 방향 벡터
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // y축 이동을 무시 (수평 이동만 고려)

            // 방향 벡터의 정규화
            Vector3 desiredDirection = direction.normalized;

            // 현재 방향에서 목표 방향으로 Slerp를 이용한 부드러운 보간
            Vector3 moveDirection = Vector3.Slerp(transform.forward, desiredDirection, slerpSpeed * Time.deltaTime);

            // 캐릭터 이동
            cc.Move(moveDirection * moveSpeed * Time.deltaTime);

            // 캐릭터의 방향을 보간된 방향으로 업데이트
            if (moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }
}
