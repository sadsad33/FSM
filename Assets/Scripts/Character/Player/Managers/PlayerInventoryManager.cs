using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInventoryManager : MonoBehaviour {
        //[SerializeField] List<Item> playerInventorySlots;
        public int reservedIndex;
        [SerializeField] List<Item> playerConsumableItemInventorySlots;
        [SerializeField] List<Item> playerWeaponInventorySlots;
        [SerializeField] List<Item> playerHelmetInventorySlots;
        [SerializeField] List<Item> playerChestArmorInventorySlots;
        [SerializeField] List<Item> playerGuntletsInventorySlots;
        [SerializeField] List<Item> playerGreavesInventorySlots;
        [SerializeField] List<Item> playerAccessoryInventorySlots;


        private void Awake() {
            //playerInventorySlots = new List<Item>();
            playerConsumableItemInventorySlots = new();
            playerWeaponInventorySlots = new();
            playerHelmetInventorySlots = new();
            playerChestArmorInventorySlots = new();
            playerGuntletsInventorySlots = new();
            playerGreavesInventorySlots = new();
            playerAccessoryInventorySlots = new();
        }

        public void ItemCategoryReader(Enums.ItemCategoryCode itemCategory, ref List<Item> refList) {
            switch (itemCategory) {
                case Enums.ItemCategoryCode.Consumable_Countable:
                case Enums.ItemCategoryCode.Consumable_Countless:
                    refList = playerConsumableItemInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Weapon_Melee:
                case Enums.ItemCategoryCode.Equipment_Weapon_Range:
                case Enums.ItemCategoryCode.Equipment_Weapon_Catalyst:
                    refList = playerWeaponInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Armor_Helmet:
                    refList = playerHelmetInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Armor_ChestArmor:
                    refList = playerChestArmorInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Armor_Gauntlets:
                    refList = playerGuntletsInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Armor_Greaves:
                    refList = playerGreavesInventorySlots;
                    break;

                case Enums.ItemCategoryCode.Equipment_Accessory_Cape:
                case Enums.ItemCategoryCode.Equipment_Accessory_Ring:
                    refList = playerAccessoryInventorySlots;
                    break;
            }
        }

        public void AddItemOnPlayerInventory(Item item) {
            List<Item> refList = new();
            ItemCategoryReader(item.itemCategory, ref refList);
            refList.Add(item);
        }

        public int GetPlayerInventorySlotsCount(Enums.ItemCategoryCode itemCategory) {
            List<Item> refList = new();
            ItemCategoryReader(itemCategory, ref refList);
            return refList.Count;
        }

        public Item? GetItemFromPlayerInventorySlots(Enums.ItemCategoryCode itemCategory, int index) {
            List<Item> refList = new();
            ItemCategoryReader(itemCategory, ref refList);
            //Debug.Log(refList[index]);
            return refList[index];
        }

        public void AddToInventory(Enums.ItemCategoryCode itemCategory, int index, Func<Item> addThisItem) {
            List<Item> refList = new();
            ItemCategoryReader(itemCategory, ref refList);
            Item temp = addThisItem();
            //Debug.Log("리스트 길이 : " + refList.Count);
            if (temp != null)
                refList[index] = temp;
            else
                refList[index] = null;
        }

        public void ResizeInventory(Enums.ItemCategoryCode itemCategory) {
            List<Item> refList = new();
            ItemCategoryReader(itemCategory, ref refList);
            for (int i = 0; i < refList.Count; i++) {
                if (refList[i] == null) {
                    refList.Remove(refList[i]);
                }
            }
        }

        //public Item SwapItem(Item receivedItem) {
        //    int typeIndex = ItemCategoryReader(receivedItem.itemCategory);
        //    Item temp = playerInventorySlots[typeIndex][reservedIndex];
        //    playerInventorySlots[typeIndex][reservedIndex] = receivedItem;
        //    return temp;
        //}
    }
}