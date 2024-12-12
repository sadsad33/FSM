using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInventoryManager : MonoBehaviour {
        [SerializeField] List<Item> playerInventorySlots;

        private void Awake() {
            playerInventorySlots = new List<Item>();
        }

        public void AddItemOnPlayerInventory(Item item) {
            playerInventorySlots.Add(item);
        }

        public int GetPlayerInventorySlotsCount() {
            Debug.Log("플레이어 인벤토리 슬롯 수 : " + playerInventorySlots.Count);
            return playerInventorySlots.Count;
        }

        public Item GetItemFromPlayerInventorySlots(int index) {
            return playerInventorySlots[index];
        }
    }
}