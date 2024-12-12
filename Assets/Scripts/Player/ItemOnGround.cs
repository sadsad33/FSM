using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class ItemOnGround : Interactable {
        public Item item;
        
        public override void Interact() {
            PlayerManager player = PlayerUIManager.instance.player;
            player.playerInventoryManager.AddItemOnPlayerInventory(item);
            
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("LootItem", player.isPerformingAction);
            
            PlayerUIManager.instance.interactionPopUp.SetActive(false);
            PlayerUIManager.instance.itemPopUpMessage.text = item.itemName;
            PlayerUIManager.instance.itemPopUpImage.sprite = item.itemIcon;
            PlayerUIManager.instance.itemPopUpImage.color = new Color(1f, 1f, 1f, 1f);
            PlayerUIManager.instance.itemPopUp.SetActive(true);

            if (PlayerUIManager.instance.inventoryUI.GetComponent<InventoryWindow>().CheckSlotsCount()) {
                PlayerUIManager.instance.inventoryUI.GetComponent<InventoryWindow>().InstantiateItemSlot();
                PlayerUIManager.instance.inventoryUI.GetComponent<InventoryWindow>().SetItemOnItemSlots();
            }

            Destroy(transform.parent.gameObject);
        }  
    }
}