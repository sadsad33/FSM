using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterController cc;
    public Rigidbody rigidbody;

    public bool isInvulnerable;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouched;

    public bool isClimbing;
    public bool isOnLadderTopEdge;
    public bool isOnLadderBottomEdge;

    public bool isPerformingAction;
    public bool isAttacking;
    public bool isMoving;

    public bool canAttackDuringAction;
    public bool canRotateDuringAction;

    public bool canSliding;
    public bool canDoComboAttack;

    public float moveSpeed;
    public float rotationSpeed;

    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;
    public bool rightFootUp;

    protected virtual void Awake() {
        cc = GetComponent<CharacterController>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Start() {

    }

    protected virtual void Update() {
        if (rightFoot.position.y > leftFoot.position.y) rightFootUp = true;
        else rightFootUp = false;
    }


}
