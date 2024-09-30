using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    [SerializeField]
    Transform targetTransform; // Ÿ���� Ʈ������
    public Transform cameraTransform;
    Vector3 cameraTransformPosition; // ī�޶� ������Ʈ�� 3���� ��ǥ�� �����ϱ����� ����

    public Transform cameraPivotTransform;
    public LayerMask obstacleLayer;
    [SerializeField] PlayerManager player;

    public Transform myTransform;
    Vector3 cameraFollowVelocity = Vector3.zero;

    public float lookSpeed;
    public float groundedFollowSpeed = 20f;
    public float inAirFollowSpeed = 20f;
    public float pivotSpeed;

    private float cameraTargetZPosition;
    private float cameraDefaultZPosition; // ī�޶� ������Ʈ�� ���� ī�޶��� ���� ��ǥ

    private float lookAngle;
    private float pivotAngle;

    public float minimumPivot = -35f;
    public float maximumPivot = 35f;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        myTransform = transform;
        cameraDefaultZPosition = cameraTransform.localPosition.z;
    }

    public void AssignCameraToPlayer(PlayerManager player) {
        this.player = player;
        targetTransform = player.transform;
    }

    public void FollowTarget(float delta) {
        if (player.isGrounded) {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, groundedFollowSpeed * Time.deltaTime);
            myTransform.position = targetPosition;
        }
        else {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, inAirFollowSpeed * Time.deltaTime);
            myTransform.position = targetPosition;
        }
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput) {
        lookAngle += (mouseXInput * lookSpeed) * delta;
        pivotAngle -= (mouseYInput * pivotSpeed) * delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollision(float delta) {
        cameraTargetZPosition = cameraDefaultZPosition;
        RaycastHit hit;
        Vector3 directionFromPlayerToCamera = cameraTransform.position - cameraPivotTransform.position;
        directionFromPlayerToCamera.Normalize();

        Debug.DrawRay(cameraPivotTransform.position, directionFromPlayerToCamera, Color.magenta);
        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, directionFromPlayerToCamera, out hit, Mathf.Abs(cameraTargetZPosition), obstacleLayer)) {
            float distanceBetweenPlayerAndObstacle = Vector3.Distance(cameraPivotTransform.position, hit.point);

            // cameraTargetZPosition �� cameraPivotTransform �� ���������� MainCamera Object�� ���� ��ǥ
            // ���������δ� �÷��̾�� ī�޶� ������ �Ÿ��� �����ϴ� ����
            // ī�޶�� ������ �÷��̾��� ��ڿ� �־�� �ϹǷ� �� ���� �׻� ������ �ȴ�.
            cameraTargetZPosition = -(distanceBetweenPlayerAndObstacle - cameraCollisionOffset);
        }

        if (Mathf.Abs(cameraTargetZPosition) < minimumCollisionOffset) {
            cameraTargetZPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, cameraTargetZPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
}