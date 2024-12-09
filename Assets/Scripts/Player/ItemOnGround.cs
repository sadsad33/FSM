using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class ItemOnGround : Interactable {
        public Item item;
        
        public override void Interact() {
            PlayerManager player = PlayerUIManager.instance.player;
            player.playerInventoryManager.AddItemOnPlayerInventory(item);
        }  
    }
}