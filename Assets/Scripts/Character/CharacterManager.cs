using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterController cc;
    public bool isInvulnerable;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouched;
    
    public bool isPerformingAction;
    public bool isAttacking;
    public bool isMoving;

    public bool canAttackDuringAction;
    public bool canDoCombo;

    public bool actionOverride;
    public bool canSliding;
    public bool canDoComboAttack;

    public float moveSpeed;
    public float rotationSpeed;

    protected virtual void Awake() {
        cc = GetComponent<CharacterController>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
    }

    protected virtual void Start() {
        
    }

    protected virtual void Update() {
        
    }

    
}
