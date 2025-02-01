using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KBH {
    public class EquipmentWindow : MonoBehaviour {
        public Transform rightHandSlotsUI;
        [SerializeField] ItemSlot[] rightHandSlots;
        public Transform leftHandSlotsUI;
        [SerializeField] ItemSlot[] leftHandSlots;
        public Transform armorSlotsUI;
        [SerializeField] ItemSlot[] armorSlots;
        public Transform accessorySlotsUI;
        [SerializeField] ItemSlot[] accessorySlots;
        public Transform consumableSlotsUI;
        [SerializeField] ItemSlot[] consumableSlots;

        PlayerEquipmentManager playerEquipment;

        public bool rightHandIsSelected;
        public bool leftHandIsSelected;
        //public bool armorIsSelected;
        public bool helmetIsSelected;
        public bool chestArmorIsSelected;
        public bool gauntletsIsSelected;
        public bool greavesIsSelected;
        public bool accessoryIsSelected;
        public bool consumableIsSelected;
        public int selectedIndex;
        public Func<Item> getSelectedItem;

        private void Awake() {
            
            rightHandSlotsUI = transform.GetChild(0).GetChild(1);
            leftHandSlotsUI = transform.GetChild(0).GetChild(2);
            armorSlotsUI = transform.GetChild(0).GetChild(3);
            accessorySlotsUI = transform.GetChild(0).GetChild(4);
            consumableSlotsUI = transform.GetChild(0).GetChild(5);
            playerEquipment = PlayerUIManager.instance.player.playerEquipmentManager;
            SetItemSlotClickEvents();
            LoadItemsOnEquipmentWindow();
        }

        // Closure : 무명메서드나 람다식이 그것을 정의(define)하고 있는 메서드(outer method)의 로컬변수(Outer 파라미터 포함)를 사용하고 있을 때, 그 무명메서드 혹은 람다식을 Closure라 부른다
        private void SetItemSlotClickEvents() {
            consumableSlots = consumableSlotsUI.transform.GetComponentsInChildren<ItemSlot>();
            InventoryWindow invWin = PlayerUIManager.instance.inventoryUI.GetComponent<InventoryWindow>();
            int i = 0;
            for (i = 0; i < consumableSlots.Length; i++) {
                int index = i; // 클로저를 위한 변수
                consumableSlots[i].itemSlotButtonLeftClickListener += () => invWin.InventoryPageIndex = 0;
                consumableSlots[i].itemSlotButtonLeftClickListener += PlayerUIManager.instance.SetHandlerToInventorySlot;
                consumableSlots[i].itemSlotButtonLeftClickListener += () => {
                    selectedIndex = index;
                    consumableIsSelected = true;
                    getSelectedItem = () => { return consumableSlots[index].item; };
                };

                consumableSlots[i].itemSlotButtonRightClickListener += () => {
                    if (consumableSlots[index].item == null) return;
                    PlayerUIManager.instance.player.playerInventoryManager.AddItemOnPlayerInventory(consumableSlots[index].item);
                    invWin.inventoryPages[0].InstantiateItemSlot();
                    invWin.inventoryPages[0].SetItemOnItemSlots();
                    consumableSlots[index].ClearItem();
                    playerEquipment.playerConsumableEquipments[index] = null;
                    LoadItemsOnEquipmentWindow();
                };
            }

            rightHandSlots = rightHandSlotsUI.transform.GetComponentsInChildren<ItemSlot>();
            leftHandSlots = leftHandSlotsUI.transform.GetComponentsInChildren<ItemSlot>();
            for (i = 0; i < rightHandSlots.Length; i++) {
                int index = i;
                rightHandSlots[i].itemSlotButtonLeftClickListener += () => invWin.InventoryPageIndex = 1;
                rightHandSlots[i].itemSlotButtonLeftClickListener += PlayerUIManager.instance.SetHandlerToInventorySlot;
                rightHandSlots[i].itemSlotButtonLeftClickListener += () => {
                    selectedIndex = index;
                    rightHandIsSelected = true;
                    getSelectedItem = () => { return rightHandSlots[index].item; };
                    //playerEquipment.playerRightHandEquipments[index] = AddThis(PlayerUIManager.instance.swapItem, getSelectedItem());
                };

                rightHandSlots[i].itemSlotButtonRightClickListener += () => {
                    if (rightHandSlots[index].item == null) return;
                    PlayerUIManager.instance.player.playerInventoryManager.AddItemOnPlayerInventory(rightHandSlots[index].item);
                    invWin.inventoryPages[1].InstantiateItemSlot(); // 현재 인벤토리 페이지의 슬롯수를 확인, 부족하다면 풀링
                    invWin.inventoryPages[1].SetItemOnItemSlots();
                    rightHandSlots[index].ClearItem(); // 아이템 이미지 제거
                    playerEquipment.playerRightHandEquipments[index] = null; // 장비매니저에 해당 정보를 비움
                    if (index == playerEquipment.playerCurrentRightHandSlotIndex) { // 현재 손에 들고있는 장비라면 바로 적용
                        playerEquipment.playerRightHandSlot.UnEquipItemOnSlot();
                    }
                    LoadItemsOnEquipmentWindow(); // 장비창에 반영
                };

                leftHandSlots[i].itemSlotButtonLeftClickListener += () => invWin.InventoryPageIndex = 1;
                leftHandSlots[i].itemSlotButtonLeftClickListener += PlayerUIManager.instance.SetHandlerToInventorySlot;
                leftHandSlots[i].itemSlotButtonLeftClickListener += () => {
                    selectedIndex = index;
                    leftHandIsSelected = true;
                    getSelectedItem = () => { return leftHandSlots[index].item; };
                };
                leftHandSlots[i].itemSlotButtonRightClickListener += () => {
                    if (leftHandSlots[index].item == null) return;
                    PlayerUIManager.instance.player.playerInventoryManager.AddItemOnPlayerInventory(leftHandSlots[index].item);
                    invWin.inventoryPages[1].InstantiateItemSlot();
                    invWin.inventoryPages[1].SetItemOnItemSlots();
                    leftHandSlots[index].ClearItem();
                    playerEquipment.playerLeftHandEquipments[index] = null;
                    if (index == playerEquipment.playerCurrentLeftHandSlotIndex) {
                        playerEquipment.playerLeftHandSlot.UnEquipItemOnSlot();
                    }
                    LoadItemsOnEquipmentWindow();
                };
            }

            armorSlots = armorSlotsUI.transform.GetComponentsInChildren<ItemSlot>();
            for (i = 0; i < armorSlots.Length; i++) {
                int index = i;
                //Debug.Log(index);
                armorSlots[i].itemSlotButtonLeftClickListener += () => {
                    invWin.InventoryPageIndex = index + 2;
                };
                armorSlots[i].itemSlotButtonLeftClickListener += PlayerUIManager.instance.SetHandlerToInventorySlot;
                armorSlots[i].itemSlotButtonLeftClickListener += () => {
                    selectedIndex = index;
                    getSelectedItem = () => { return armorSlots[index].item; };
                };
                armorSlots[i].itemSlotButtonLeftClickListener += () => {
                    switch (index+2) {
                        case 2:
                            helmetIsSelected = true;
                            break;
                        case 3:
                            chestArmorIsSelected = true;
                            break;
                        case 4:
                            gauntletsIsSelected = true;
                            break;
                        case 5:
                            greavesIsSelected = true;
                            break;
                    }
                };

                armorSlots[i].itemSlotButtonRightClickListener += () => {
                    PlayerUIManager.instance.player.playerInventoryManager.AddItemOnPlayerInventory(armorSlots[index].item);
                    invWin.inventoryPages[index + 2].InstantiateItemSlot();
                    invWin.inventoryPages[index + 2].SetItemOnItemSlots();
                    armorSlots[index].ClearItem();
                    playerEquipment.playerArmorEquipments[index] = null;
                    switch (index + 2) {
                        case 2:
                            playerEquipment.SetHelmet();
                            break;
                        case 3:
                            playerEquipment.SetChestArmor();
                            break;
                        case 4:
                            playerEquipment.SetGauntlets();
                            break;
                        case 5:
                            playerEquipment.SetGreaves();
                            break;
                    }
                    LoadItemsOnEquipmentWindow();
                };
            }

            accessorySlots = accessorySlotsUI.transform.GetComponentsInChildren<ItemSlot>();
            for (i = 0; i < accessorySlots.Length; i++) {
                int index = i;
                accessorySlots[i].itemSlotButtonLeftClickListener += () => invWin.InventoryPageIndex = 6;
                accessorySlots[i].itemSlotButtonLeftClickListener += PlayerUIManager.instance.SetHandlerToInventorySlot;
                accessorySlots[i].itemSlotButtonLeftClickListener += () => {
                    selectedIndex = index;
                    accessoryIsSelected = true;
                    getSelectedItem = () => { return accessorySlots[index].item; };
                };

                accessorySlots[i].itemSlotButtonRightClickListener += () => {
                    PlayerUIManager.instance.player.playerInventoryManager.AddItemOnPlayerInventory(accessorySlots[i].item);
                    invWin.inventoryPages[6].InstantiateItemSlot();
                    invWin.inventoryPages[6].SetItemOnItemSlots();
                    accessorySlots[index].ClearItem();
                    playerEquipment.playerAccessoryEquipments[index] = null;
                    // TODO
                    // 현재 장착하고 있는 악세서리 모델을 온/오프 시켜주는 메서드 추가해줘야함
                    LoadItemsOnEquipmentWindow();
                };
            }
        }

        // 플레이어의 EquipmentManager로 부터 장착하고 있는 장비 목록을 가져와 UI에 반영
        public void LoadItemsOnEquipmentWindow() {
            for (int i = 0; i < playerEquipment.playerRightHandEquipments.Count; i++) {
                rightHandSlots[i].AddItem(playerEquipment.playerRightHandEquipments[i]);
                leftHandSlots[i].AddItem(playerEquipment.playerLeftHandEquipments[i]);
            }

            for (int i = 0; i < playerEquipment.playerArmorEquipments.Count; i++) {
                armorSlots[i].AddItem(playerEquipment.playerArmorEquipments[i]);
            }

            for (int i = 0; i < playerEquipment.playerConsumableEquipments.Count; i++) {
                consumableSlots[i].AddItem(playerEquipment.playerConsumableEquipments[i]);
            }

            for (int i = 0; i < playerEquipment.playerAccessoryEquipments.Count; i++) {
                accessorySlots[i].AddItem(playerEquipment.playerAccessoryEquipments[i]);
            }
        }

        public void ResetSelectedFlag() {
            rightHandIsSelected = false;
            leftHandIsSelected = false;
            helmetIsSelected = false;
            chestArmorIsSelected = false;
            gauntletsIsSelected = false;
            greavesIsSelected = false;
            consumableIsSelected = false;
            accessoryIsSelected = false;
        }

        //public Item AddThis(Func<Item, Item> addThis, Item send) {
        //    return addThis(send);
        //}
    }
}