using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionState : IState {
        protected PlayerManager player;
        protected PlayerInteractionManager playerInteraction;
        protected Interactable curInteractable;
        public virtual void Enter(CharacterManager character) {
            player = character as PlayerManager;
            playerInteraction = player.playerInteractionManager;
            //Debug.Log("Player Current Interaction State : " + GetType());
            //Debug.Log("Player Current Interactable : " + curInteractable);
        }

        public virtual void Stay(CharacterManager character) {
            HandleInput(character);
        }

        public virtual void Exit(CharacterManager character) {
            //Debug.Log("Player Exit Interaction State : " + GetType());
        }

        public virtual void HandleInput(CharacterManager character) {
        }

    }
}
