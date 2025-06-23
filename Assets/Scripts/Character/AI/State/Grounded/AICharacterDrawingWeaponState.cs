using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterDrawingWeaponState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            aiCharacter.aiStatsManager.hasDrawnWeapon = true;
            aiCharacter.aiAnimatorManager.PlayAnimation("Draw Sword", aiCharacter.isPerformingAction);
            aiCharacter.aiEquipmentManager.rightHandSlot.EquipItemOnSlot(aiCharacter.aiEquipmentManager.GetAICurrentRightWeapon());
            aiCharacter.aiEquipmentManager.LoadRightWeaponDamageCollider();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            if (!aiCharacter.isPerformingAction) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
            }
        }
    }
}
