using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintingState : PlayerMovingState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        moveSpeedModifier = 1.6f;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.RunningStateTimer += Time.deltaTime;
        //HandleMovement();
        player.playerAnimatorManager.animator.SetFloat("Vertical", 2.0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
        // �̵� Ű + Left Shift => Sprint
        // ���� ���¿��� Left Shift �� �̵� Ű ���� �ϳ��� �Է��� ���ŵȴٸ� Medium Stopping State�� ����
        // �̶� �̵�Ű �Է��� ���� �����ϸ�, Sprinting State�� ����Ǹ鼭 SprintInputTimer �� 0���� �ʱ�ȭ������, ���� Left Shift �Է��� true �̹Ƿ� SprintInputTimer �� �ٽ� ���ư�
        // Left Shift ���� �ڴʰ� ���� ���� ���� ���, InputTimer �� �ð��� 0.3 �� �̸��̶�� RollFlag �� true �� �Ǿ� Rolling State�� ���̵�
    }

    public override void HandleInput() {
        base.HandleInput();
        if (moveAmount <= 0f )
            player.pmsm.ChangeState(player.pmsm.mediumStoppingState);
        else if(!player.playerInputManager.SprintInput)player.pmsm.ChangeState(player.pmsm.runningState);
    }

    protected override void HandleRotation() {
        base.HandleRotation();
        Quaternion tr = Quaternion.LookRotation(lookingDirection);
        Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, player.rotationSpeed * Time.deltaTime);
        player.transform.rotation = targetRotation;
    }

    protected override void HandleMovement() {
        base.HandleMovement();
        float speed = player.moveSpeed * moveSpeedModifier;
        currentMovingSpeed = speed;
        moveDirection *= speed;
        player.cc.Move(moveDirection / 200f);
    }
}
