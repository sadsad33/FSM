using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterIdlingState : AICharacterGroundedState {

        bool hasSelectedTarget;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //if (!aiCharacter.cc.enabled) aiCharacter.cc.enabled = true;
            aiCharacter.isCombatStance = false;
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 0f);
            moveSpeedModifier = 0f;
            //Debug.Log(aiCharacter.isGrounded);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);

        }

        public override void Exit(CharacterManager character) {

        }

        public override void Thinking() {
            base.Thinking();
            if (!hasSelectedTarget) SelectTarget();
            else if (aiCharacter.aiStatsManager.hasDrawnWeapon) aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiPursuingState);
            else aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiDrawingWeaponState);
        }

        private void SelectTarget() {
            if (aiCharacter.aiEyesManager.targetAround == null && aiCharacter.aiEyesManager.targetPossible == null) return;
            else if (aiCharacter.aiEyesManager.targetAround != null) {
                aiCharacter.aiEyesManager.currentTarget = aiCharacter.aiEyesManager.targetAround;
                hasSelectedTarget = true;
            }else {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiBewaringState);
            }
        }
    }
}
