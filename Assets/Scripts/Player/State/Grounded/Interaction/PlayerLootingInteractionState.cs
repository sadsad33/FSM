using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLootingInteractionState : PlayerInteractionState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            curInteractable = playerInteraction.currentInteractable;
            Debug.Log("CurrentInteractable : " + curInteractable);
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("LootItem", player.isPerformingAction);
            curInteractable.Interact();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            curInteractable = null;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction)
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
        }
    }
}