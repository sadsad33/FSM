using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterController cc;
    public bool isInvulnerable;
    public bool isGrounded;

    public bool isPerformingAction;

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
