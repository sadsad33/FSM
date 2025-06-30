using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterStateMachine : StateMachine {

        public AICharacterIdlingState aiIdlingState;
        public AICharacterPursuingState aiPursuingState;
        public AICharacterFallingState aiFallingState;

        public AICharacterMoveToInteractingPositionState aiMoveToInteractingPositionState;
        public AICharacterLadderBottomStartInteractionState aiLadderBottomStartInteractionState;
        public AICharacterLadderBottomEndInteractionState aiLadderBottomEndInteractionState;
        public AICharacterLadderTopStartInteractionState aiLadderTopStartInteractionState;
        public AICharacterLadderTopEndInteractionState aiLadderTopEndInteractionState;

        public AICharacterLightAttackState aiLightAttackState;
        public AICharacterHeavyAttackState aiHeavyAttackState;

        public AICharacterRightFootUpIdlingState aiRightFootUpIdlingState;
        public AICharacterLeftFootUpIdlingState aiLeftFootUpIdlingState;

        public AICharacterClimbingUpState aiClimbingUpState;
        public AICharacterClimbingDownState aiClimbingDownState;

        public AICharacterDrawingWeaponState aiDrawingWeaponState;
        public AICharacterBewaringState aiBewaringState;
        public AICharacterCombatStanceState aiCombatStanceState;

        public AICharacterGroundedHitState aiGroundedHitState;

        public AICharacterHasBeenParriedState aiHasBeenParriedState;
        public AICharacterBeingRipostedState aiBeingRipostedState;
        public AICharacterBeingBackstabbedState aiBeingBackstabbedState;

        public AICharacterRipostedGetUpState aiRipostedGetUpState;
        public AICharacterBackstabbedGetUpState aiBackstabbedGetUpState;
        public AICharacterStateMachine(CharacterManager character) : base(character) {
            aiIdlingState = new();
            aiPursuingState = new();
            aiFallingState = new();

            aiMoveToInteractingPositionState = new();
            aiLadderBottomStartInteractionState = new();
            aiLadderBottomEndInteractionState = new();
            aiLadderTopStartInteractionState = new();
            aiLadderTopEndInteractionState = new();

            aiRightFootUpIdlingState = new();
            aiLeftFootUpIdlingState = new();

            aiClimbingUpState = new();
            aiClimbingDownState = new();

            aiDrawingWeaponState = new();
            aiBewaringState = new();
            aiCombatStanceState = new();

            aiLightAttackState = new();
            aiHeavyAttackState = new();

            aiGroundedHitState = new();

            aiHasBeenParriedState = new();
            aiBeingRipostedState = new();
            aiBeingBackstabbedState = new();

            aiRipostedGetUpState = new();
            aiBackstabbedGetUpState = new();
        }

        public override void ChangeState(IState newState) {
            base.ChangeState(newState);
        }
    }
}
