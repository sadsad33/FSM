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
        HandleGroundedMovement();
        player.playerAnimatorManager.animator.SetFloat("Vertical", 2.0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
        // �̵� Ű + Left Shift => Sprint
        // ���� ���¿��� Left Shift �� �̵� Ű ���� �ϳ��� �Է��� ���ŵȴٸ� Medium Stopping State�� ����
        // �̶� �̵�Ű �Է��� ���� �����ϸ�, Sprinting State�� ����Ǹ鼭 SprintInputTimer �� 0���� �ʱ�ȭ������, ���� Left Shift �Է��� true �̹Ƿ� SprintInputTimer �� �ٽ� ���ư�
        // Left Shift ���� �ڴʰ� ���� ���� ���� ���, InputTimer �� �ð��� 0.3 �� �̸��̶�� RollFlag �� true �� �Ǿ� Rolling State�� ���̵�

        player.playerInputManager.SprintInputTimer = 0f;
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.playerInputManager.SprintInput || moveAmount <= 0f) {
                player.pmsm.ChangeState(player.pmsm.mediumStoppingState);
        }
    }

    protected override void HandleGroundedMovement() {
        base.HandleGroundedMovement();
        float speed = player.moveSpeed * moveSpeedModifier;
        moveDirection *= speed;
        moveDirection /= 200f;
        player.cc.Move(moveDirection);
    }
}
