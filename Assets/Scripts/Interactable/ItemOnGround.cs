using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class ItemOnGround : Interactable {
        public Item item;
        
        public override void Interact(CharacterManager character) {
            //Debug.Log("아이템 줍기");
            if (character is PlayerManager) {
                PlayerManager player = character.transform.GetComponent<PlayerManager>();
                player.playerInventoryManager.AddItemOnPlayerInventory(item);

                PlayerUIManager.instance.interactionPopUp.SetActive(false);
                PlayerUIManager.instance.itemPopUpMessage.text = item.itemName;
                PlayerUIManager.instance.itemPopUpImage.sprite = item.itemIcon;
                PlayerUIManager.instance.itemPopUpImage.color = new Color(1f, 1f, 1f, 1f);
                PlayerUIManager.instance.itemPopUp.SetActive(true);
                InventoryWindow invWin = PlayerUIManager.instance.inventoryUI.GetComponent<InventoryWindow>();
                switch (item.itemCategory) {
                    case ItemCategory.Consumable_Countless:
                    case ItemCategory.Consumable_Countable:
                        invWin.inventoryPages[0].InstantiateItemSlot();
                        invWin.inventoryPages[0].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Weapon_Melee:
                    case ItemCategory.Equipment_Weapon_Range:
                    case ItemCategory.Equipment_Weapon_Catalyst:
                        invWin.inventoryPages[1].InstantiateItemSlot();
                        invWin.inventoryPages[1].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Armor_Helmet:
                        invWin.inventoryPages[2].InstantiateItemSlot();
                        invWin.inventoryPages[2].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Armor_ChestArmor:
                        invWin.inventoryPages[3].InstantiateItemSlot();
                        invWin.inventoryPages[3].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Armor_Guntlets:
                        invWin.inventoryPages[4].InstantiateItemSlot();
                        invWin.inventoryPages[4].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Armor_Greaves:
                        invWin.inventoryPages[5].InstantiateItemSlot();
                        invWin.inventoryPages[5].SetItemOnItemSlots();
                        break;
                    case ItemCategory.Equipment_Accessory_Ring:
                    case ItemCategory.Equipment_Accessory_Cape:
                        invWin.inventoryPages[6].InstantiateItemSlot();
                        invWin.inventoryPages[6].SetItemOnItemSlots();
                        break;
                }

                player.playerInteractionManager.currentInteractable = null;
                Destroy(transform.parent.gameObject);
            }
        }  
    }
}