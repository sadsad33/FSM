using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class InventoryWindow : MonoBehaviour {
        public GameObject itemSlotModel;

        public GameObject inventoryIndex;
        public GameObject inventoryPage;
        public List<GameObject> itemSlots = new();

        private void Start() {
            if (CheckSlotsCount()) InstantiateItemSlot();
            inventoryIndex = transform.GetChild(0).GetChild(0).gameObject;
            inventoryPage = transform.GetChild(0).GetChild(1).gameObject;
        }

        public bool CheckSlotsCount() {
            return itemSlots.Count <= PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount();
        }

        public void InstantiateItemSlot() {
            itemSlots.Add(Instantiate(itemSlotModel, inventoryPage.transform));
        }

        public void SetItemOnItemSlots() {
            for (int i = 0; i < PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount(); i++) {
                if (PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(i) != null) {
                    Item item = PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(i);
                    ItemSlot itemSlot = itemSlots[i].GetComponent<ItemSlot>();
                    itemSlot.AddItem(item);
                }
            }
        }
    }
}