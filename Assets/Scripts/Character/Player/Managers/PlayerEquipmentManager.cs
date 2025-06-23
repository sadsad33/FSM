using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerEquipmentManager : CharacterEquipmentManager {
        public PlayerManager player;
        EquipmentWindow equipWin;

        protected override void Awake() {
            base.Awake();
            player = GetComponent<PlayerManager>();
            equipWin = PlayerUIManager.instance.equipmentUI.GetComponent<EquipmentWindow>();
            rightHandEquipments = new(3) { null, null, null };
            leftHandEquipments = new(3) { null, null, null };
            characterArmorEquipments = new(4) { null, null, null, null };
        }

        protected override void Start() {
            base.Start();

            SetHelmet();
            SetChestArmor();
            SetGauntlets();
            SetGreaves();
        }

        public void ChangeRightHandWeapon() {
            if (rightHandSlot.GetItemOnSlot() != null)
                rightHandSlot.UnEquipItemOnSlot();
            currentRightHandSlotIndex++;
            if (currentRightHandSlotIndex >= 3)
                currentRightHandSlotIndex -= 3;
            rightHandSlot.EquipItemOnSlot(rightHandEquipments[currentRightHandSlotIndex]);
            LoadRightWeaponDamageCollider();
        }

        public void AddToEquipments(Item equipThis) {
            // 갑옷에도 양손 처럼 모델 리로드 메서드 추가 해야함
            if (equipWin.rightHandIsSelected) {
                rightHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == currentRightHandSlotIndex) {
                    rightHandSlot.UnEquipItemOnSlot();
                    rightHandSlot.EquipItemOnSlot(rightHandEquipments[currentRightHandSlotIndex]);
                    LoadRightWeaponDamageCollider();
                }
            } else if (equipWin.leftHandIsSelected) {
                leftHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == currentLeftHandSlotIndex) {
                    leftHandSlot.UnEquipItemOnSlot();
                    leftHandSlot.EquipItemOnSlot(leftHandEquipments[currentLeftHandSlotIndex]);
                    LoadLeftWeaponDamageCollider();
                }
            } else if (equipWin.helmetIsSelected) {
                characterArmorEquipments[0] = equipThis as ArmorItem;
                SetHelmet();
            } else if (equipWin.chestArmorIsSelected) {
                characterArmorEquipments[1] = equipThis as ArmorItem;
                SetChestArmor();
            } else if (equipWin.gauntletsIsSelected) {
                characterArmorEquipments[2] = equipThis as ArmorItem;
                SetGauntlets();
            } else if (equipWin.greavesIsSelected) {
                characterArmorEquipments[3] = equipThis as ArmorItem;
                SetGreaves();
            } else if (equipWin.consumableIsSelected) {
                consumableEquipments[equipWin.selectedIndex] = equipThis;
            } else if (equipWin.accessoryIsSelected) {
                consumableEquipments[equipWin.selectedIndex] = equipThis;
            }
        }
    }
}