using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerEquipmentManager : MonoBehaviour {
        public PlayerManager player;
        EquipmentWindow equipWin;
        public bool changeNow;
        [Header("RightHand")]
        public List<Item> playerRightHandEquipments;
        public PlayerHandSlot playerRightHandSlot;
        public int playerCurrentRightHandSlotIndex;

        [Header("LeftHand")]
        public List<Item> playerLeftHandEquipments;
        public PlayerHandSlot playerLeftHandSlot;
        public int playerCurrentLeftHandSlotIndex;

        [Header("Armor")]
        public List<ArmorItem> playerArmorEquipments;
        public Transform playerFaceModel;
        public Transform playerHelmetModel;
        public Transform playerChestArmorModel;
        //public Transform playerCapeModel;
        public Transform playerGauntletsModel;
        public Transform playerGreavesModel;
        public Transform playerBootsModel;

        [Header("Consumable")]
        public List<Item> playerConsumableEquipments;

        [Header("Accessory")]
        public List<Item> playerAccessoryEquipments;

        private void Awake() {
            player = GetComponent<PlayerManager>();
            equipWin = PlayerUIManager.instance.equipmentUI.GetComponent<EquipmentWindow>();
        }

        private void Start() {
            if (playerRightHandEquipments[playerCurrentRightHandSlotIndex] != null)
                playerRightHandSlot.EquipItemOnSlot(playerRightHandEquipments[playerCurrentRightHandSlotIndex]);

            if (playerLeftHandEquipments[playerCurrentLeftHandSlotIndex] != null)
                playerLeftHandSlot.EquipItemOnSlot(playerLeftHandEquipments[playerCurrentLeftHandSlotIndex]);

            SetHelmet();
            SetChestArmor();
            SetGauntlets();
            SetGreaves();
        }

        public void ChangeRightHandWeapon() {
            if (playerRightHandSlot.currentItemOnSlot != null)
                playerRightHandSlot.UnEquipItemOnSlot();
            playerCurrentRightHandSlotIndex++;
            if (playerCurrentRightHandSlotIndex >= 3)
                playerCurrentRightHandSlotIndex -= 3;
            playerRightHandSlot.EquipItemOnSlot(playerRightHandEquipments[playerCurrentRightHandSlotIndex]);
        }

        public void AddToEquipments(Item equipThis) {
            // 갑옷에도 양손 처럼 모델 리로드 메서드 추가 해야함
            if (equipWin.rightHandIsSelected) {
                playerRightHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == playerCurrentRightHandSlotIndex) {
                    playerRightHandSlot.UnEquipItemOnSlot();
                    playerRightHandSlot.EquipItemOnSlot(playerRightHandEquipments[playerCurrentRightHandSlotIndex]);
                }
            } else if (equipWin.leftHandIsSelected) {
                playerLeftHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == playerCurrentLeftHandSlotIndex) {
                    playerLeftHandSlot.UnEquipItemOnSlot();
                    playerLeftHandSlot.EquipItemOnSlot(playerLeftHandEquipments[playerCurrentLeftHandSlotIndex]);
                }
            } else if (equipWin.helmetIsSelected) {
                playerArmorEquipments[0] = equipThis as ArmorItem;
                SetHelmet();
            } else if (equipWin.chestArmorIsSelected) {
                playerArmorEquipments[1] = equipThis as ArmorItem;
                SetChestArmor();
            } else if (equipWin.gauntletsIsSelected) {
                playerArmorEquipments[2] = equipThis as ArmorItem;
                SetGauntlets();
            } else if (equipWin.greavesIsSelected) {
                playerArmorEquipments[3] = equipThis as ArmorItem;
                SetGreaves();
            } else if (equipWin.consumableIsSelected) {
                playerConsumableEquipments[equipWin.selectedIndex] = equipThis;
            } else if (equipWin.accessoryIsSelected) {
                playerConsumableEquipments[equipWin.selectedIndex] = equipThis;
            }
        }

        public void SetHelmet() {
            for (int i = 0; i < playerHelmetModel.childCount; i++) {
                playerHelmetModel.GetChild(i).gameObject.SetActive(false);
            }
            if (playerArmorEquipments[0] == null) playerHelmetModel.GetChild(0).gameObject.SetActive(true);
            else playerHelmetModel.GetChild(playerArmorEquipments[0].armorCode).gameObject.SetActive(true);
        }

        public void SetChestArmor() {
            for (int i = 0; i < playerChestArmorModel.childCount; i++) {
                playerChestArmorModel.GetChild(i).gameObject.SetActive(false);
            }
            if (playerArmorEquipments[1] == null) playerChestArmorModel.GetChild(0).gameObject.SetActive(true);
            else playerChestArmorModel.GetChild(playerArmorEquipments[1].armorCode).gameObject.SetActive(true);
        }

        public void SetGauntlets() {
            for (int i = 0; i < playerGauntletsModel.childCount; i++) {
                playerGauntletsModel.GetChild(i).gameObject.SetActive(false);
            }
            if (playerArmorEquipments[2] == null) playerGauntletsModel.GetChild(0).gameObject.SetActive(true);
            else playerGauntletsModel.GetChild(playerArmorEquipments[2].armorCode).gameObject.SetActive(true);
        }

        public void SetGreaves() {
            for (int i = 0; i < playerGreavesModel.childCount; i++) {
                playerGreavesModel.GetChild(i).gameObject.SetActive(false);
                playerBootsModel.GetChild(i).gameObject.SetActive(false);
            }
            if (playerArmorEquipments[3] == null) {
                playerGreavesModel.GetChild(0).gameObject.SetActive(true);
                playerBootsModel.GetChild(0).gameObject.SetActive(true);
            } else {
                playerGreavesModel.GetChild(playerArmorEquipments[3].armorCode).gameObject.SetActive(true);
                playerBootsModel.GetChild(playerArmorEquipments[3].armorCode).gameObject.SetActive(true);
            }
        }
    }
}