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
            HandleRightHandAnimation();
            LoadRightWeaponDamageCollider();
        }

        public void ChangeLeftHandWeapon() {
            if (leftHandSlot.GetItemOnSlot() != null) leftHandSlot.UnEquipItemOnSlot();
            currentLeftHandSlotIndex++;
            if (currentLeftHandSlotIndex >= 3)
                currentLeftHandSlotIndex -= 3;
            leftHandSlot.EquipItemOnSlot(leftHandEquipments[currentLeftHandSlotIndex]);
            HandleLeftHandAnimation();
            LoadLeftWeaponDamageCollider();
        }

        public void HandleTwoHandingAnimation() {
            if (player.isTwoHanding) {
                player.isTwoHanding = false;
                player.playerAnimatorManager.PlayAnimation("Two Hand Empty", false);
            } else {
                player.isTwoHanding = true;
                player.playerAnimatorManager.PlayAnimation("Two Hand Idle", false);
            }
        }

        public void HandleRightHandAnimation() {
            if ((rightHandSlot.GetItemOnSlot() as WeaponItem).isUnarmed) {
                Debug.Log("hi");
                player.playerAnimatorManager.PlayAnimation("Right Hand Empty", false);
            } else
                player.playerAnimatorManager.PlayAnimation("Right Hand Idle", false);
        }

        public void HandleLeftHandAnimation() {
            if ((leftHandSlot.GetItemOnSlot() as WeaponItem).isUnarmed)
                player.playerAnimatorManager.PlayAnimation("Left Hand Empty", false);
            else
                player.playerAnimatorManager.PlayAnimation("Left Hand Idle", false);
        }

        public void AddToEquipments(Item equipThis) {
            // 갑옷에도 양손 처럼 모델 리로드 메서드 추가 해야함
            if (equipWin.rightHandIsSelected) {
                rightHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == currentRightHandSlotIndex) {
                    rightHandSlot.UnEquipItemOnSlot();
                    rightHandSlot.EquipItemOnSlot(rightHandEquipments[currentRightHandSlotIndex]);
                    HandleRightHandAnimation();
                    LoadRightWeaponDamageCollider();
                }
            } else if (equipWin.leftHandIsSelected) {
                leftHandEquipments[equipWin.selectedIndex] = equipThis;
                if (equipWin.selectedIndex == currentLeftHandSlotIndex) {
                    leftHandSlot.UnEquipItemOnSlot();
                    leftHandSlot.EquipItemOnSlot(leftHandEquipments[currentLeftHandSlotIndex]);
                    HandleLeftHandAnimation();
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