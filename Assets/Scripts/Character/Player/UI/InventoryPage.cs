using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class InventoryPage : MonoBehaviour {

        public Enums.ItemCategoryCode itemCategory;
        public GameObject itemSlotModel;
        public List<GameObject> itemSlots = new();

        public void InstantiateItemSlot() {
            if (itemSlots.Count <= PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount(itemCategory)){
                itemSlots.Add(Instantiate(itemSlotModel, transform));
            }
        }  

        public void SetItemOnItemSlots() {
            for (int i = 0; i < PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount(itemCategory); i++) {
                //Debug.Log(PlayerUIManager.instance.player.playerInventoryManager.GetPlayerInventorySlotsCount(itemCategory));
                if (PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(itemCategory, i) != null) {
                    //Debug.Log(i);
                    Item item = PlayerUIManager.instance.player.playerInventoryManager.GetItemFromPlayerInventorySlots(itemCategory, i);
                    ItemSlot itemSlot = itemSlots[i].GetComponentInChildren<ItemSlot>();
                    itemSlot.AddItem(item);
                } else {
                    GameObject itemSlot = itemSlots[i].transform.gameObject;
                    itemSlots.RemoveAt(i);
                    Destroy(itemSlot);
                    PlayerUIManager.instance.player.playerInventoryManager.ResizeInventory(itemCategory);
                }
            }
        }
    }
}
