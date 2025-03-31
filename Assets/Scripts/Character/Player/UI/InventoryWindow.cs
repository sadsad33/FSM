using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace KBH {
    public class InventoryWindow : MonoBehaviour {

        public bool isChangingEquipment;
        public GameObject inventoryIndex;

        [SerializeField] int inventoryPageIndex;
        public int InventoryPageIndex {
            get { return inventoryPageIndex; }
            set { inventoryPageIndex = value; }
        }
        public InventoryPage[] inventoryPages;

        private void OnEnable() {
            inventoryIndex = transform.GetChild(0).GetChild(0).gameObject;
            //Debug.Log(inventoryPageIndex);
            SetActiveInventoryWindow();
        }

        private void OnDisable() {
            if (isChangingEquipment) {
                isChangingEquipment = false;
                PlayerUIManager.instance.equipmentUI.GetComponent<EquipmentWindow>().ResetSelectedFlag();
            }
            List<GameObject> slots = inventoryPages[InventoryPageIndex].itemSlots;
            for (int i = 0; i < slots.Count; i++) {
                slots[i].GetComponentInChildren<ItemSlot>().ResetItemSlotButtonLeftClickEvent();
            }
        }

        public void SetActiveInventoryWindow() {
            inventoryPages[inventoryPageIndex].gameObject.SetActive(true);
            for (int i = 0; i < inventoryPages.Length; i++) {
                if (i == inventoryPageIndex) continue;
                inventoryPages[i].gameObject.SetActive(false);
            }
        }

        public void NextPageButtonClick() {
            inventoryPageIndex += 1;
            if (inventoryPageIndex >= inventoryPages.Length)
                inventoryPageIndex = 0;
        }

        public void PreviousPageButtonClick() {
            inventoryPageIndex -= 1;
            if (inventoryPageIndex < 0)
                inventoryPageIndex = inventoryPages.Length - 1;
        }

        public void SetItemSlotClickEvents() {
            // 장비 창의 특정 슬롯을 클릭해 인벤토리 윈도우로 온 상태라면
            // 소지품 창의 슬롯을 클릭함과 동시에 소지품 윈도우가 닫히고 장비 윈도우의 슬롯에 아이템 반영
            if (isChangingEquipment) {
                List<GameObject> slots = inventoryPages[InventoryPageIndex].itemSlots;
                PlayerManager player = PlayerUIManager.instance.player;
                EquipmentWindow equipWin = PlayerUIManager.instance.equipmentUI.GetComponent<EquipmentWindow>();
                for (int i = 0; i < slots.Count; i++) {
                    if (slots[i] != null && slots[i].GetComponentInChildren<ItemSlot>().LeftClickEventIsEmpty()) {
                        int index = i;
                        // 장비 아이템의 교환은 실질적으로 EquipmentManager 와 InventoryManager 사이에서 일어나야함
                        // 교환이 일어나게 된다면 EquipmentWindow 에는 LoadItemsOnWindow 메서드를,
                        // InventoryPage 에는 SetItemOnItemSlots 메서드로 아이템을 UI에 반영
                        slots[index].GetComponentInChildren<ItemSlot>().itemSlotButtonLeftClickListener += () => {
                            Item temp = slots[index].GetComponentInChildren<ItemSlot>().item;
                            //Debug.Log("인덱스 : " + index);
                            player.playerInventoryManager.AddToInventory(temp.itemCategory, index, equipWin.getSelectedItem);
                            player.playerEquipmentManager.AddToEquipments(temp);
                        };
                        slots[index].GetComponentInChildren<ItemSlot>().itemSlotButtonLeftClickListener += inventoryPages[InventoryPageIndex].SetItemOnItemSlots;
                        slots[index].GetComponentInChildren<ItemSlot>().itemSlotButtonLeftClickListener += equipWin.LoadItemsOnEquipmentWindow;
                        slots[index].GetComponentInChildren<ItemSlot>().itemSlotButtonLeftClickListener += PlayerUIManager.instance.CloseWindow;
                    }
                }
            }

            // TODO
            // 소지품 윈도우로 직접적으로 온 상태라면
            // 슬롯을 클릭하면 아이템의 사진과 설명이 나오는 윈도우를 화면에 출력
        }
    }
}