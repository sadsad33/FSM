using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public CharacterController cc;
    public Transform target;
    public float moveSpeed = 5f;   // �̵� �ӵ�
    public float slerpSpeed = 2f;  // ���� �ӵ�
    private void Awake() {
        cc = GetComponent<CharacterController>();
    }

    private void Update() {
        if (target != null) {
            // ���� ��ġ���� ��ǥ ��ġ�� ���ϴ� ���� ����
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // y�� �̵��� ���� (���� �̵��� ���)

            // ���� ������ ����ȭ
            Vector3 desiredDirection = direction.normalized;

            // ���� ���⿡�� ��ǥ �������� Slerp�� �̿��� �ε巯�� ����
            Vector3 moveDirection = Vector3.Slerp(transform.forward, desiredDirection, slerpSpeed * Time.deltaTime);

            // ĳ���� �̵�
            cc.Move(moveDirection * moveSpeed * Time.deltaTime);

            // ĳ������ ������ ������ �������� ������Ʈ
            if (moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }
}
