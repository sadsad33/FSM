using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class InventoryWindow : MonoBehaviour {
        public GameObject itemSlotModel;

        public GameObject inventoryIndex;
        public GameObject inventoryPage;
        public List<GameObject> itemSlots;

        private void OnEnable() {
            itemSlots = new List<GameObject>();
            inventoryIndex = transform.GetChild(0).GetChild(0).gameObject;
            inventoryPage = transform.GetChild(0).GetChild(1).gameObject;

            PlayerUIManager.instance.inventoryWindow = this;
            if (PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount() >= PlayerUIManager.instance.inventoryWindow.itemSlots.Count) {
                PlayerUIManager.instance.inventoryWindow.InstantiateItemSlot();
            }
            UpdateItemSlots();
        }

        public void InstantiateItemSlot() {
            itemSlots.Add(Instantiate(itemSlotModel, inventoryPage.transform));
        }   

        public void UpdateItemSlots() {
            for (int i = 0; i < PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount(); i++) {
                if (PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(i) != null) {
                    ItemSlot itemSlot = itemSlots[i].GetComponent<ItemSlot>();
                    Item currentItem = PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(i);
                    itemSlot.AddItem(currentItem);
                }
            }
        }
    }
}