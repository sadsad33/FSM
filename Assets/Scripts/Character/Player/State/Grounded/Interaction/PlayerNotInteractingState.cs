using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerNotInteractingState : PlayerInteractionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            curInteractable = null;
            player.playerInteractionManager.isInteracting = false;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (player.playerInputManager.InteractionInput) {
                switch (playerInteraction.currentInteractable) {
                    case null:
                        if (PlayerUIManager.instance.interactionPopUp.activeSelf)
                            PlayerUIManager.instance.interactionPopUp.SetActive(false);
                        if (PlayerUIManager.instance.itemPopUp.activeSelf)
                            PlayerUIManager.instance.itemPopUp.SetActive(false);
                        break;
                    case ItemOnGround:
                        playerInteraction.pism.ChangeState(playerInteraction.pism.lootingInteractionState);
                        break;
                    case Ladder:
                        if (!playerInteraction.currentInteractable.GetComponent<Ladder>().isTop) {
                            playerInteraction.pism.moveToInteractingPositionState.nextState = playerInteraction.pism.ladderBottomStartInteractionState;
                        } else {
                            playerInteraction.pism.moveToInteractingPositionState.nextState = playerInteraction.pism.ladderTopStartInteractionState;
                        }
                        playerInteraction.pism.ChangeState(playerInteraction.pism.moveToInteractingPositionState);
                        //player.pmsm.ChangeState(player.pmsm.rightFootUpIdlingState);
                        break;
                }
            }
        }
    }
}
