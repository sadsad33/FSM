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
            // ��� â�� Ư�� ������ Ŭ���� �κ��丮 ������� �� ���¶��
            // ����ǰ â�� ������ Ŭ���԰� ���ÿ� ����ǰ �����찡 ������ ��� �������� ���Կ� ������ �ݿ�
            if (isChangingEquipment) {
                List<GameObject> slots = inventoryPages[InventoryPageIndex].itemSlots;
                PlayerManager player = PlayerUIManager.instance.player;
                EquipmentWindow equipWin = PlayerUIManager.instance.equipmentUI.GetComponent<EquipmentWindow>();
                for (int i = 0; i < slots.Count; i++) {
                    if (slots[i] != null && slots[i].GetComponentInChildren<ItemSlot>().LeftClickEventIsEmpty()) {
                        int index = i;
                        // ��� �������� ��ȯ�� ���������� EquipmentManager �� InventoryManager ���̿��� �Ͼ����
                        // ��ȯ�� �Ͼ�� �ȴٸ� EquipmentWindow ���� LoadItemsOnWindow �޼��带,
                        // InventoryPage ���� SetItemOnItemSlots �޼���� �������� UI�� �ݿ�
                        slots[index].GetComponentInChildren<ItemSlot>().itemSlotButtonLeftClickListener += () => {
                            Item temp = slots[index].GetComponentInChildren<ItemSlot>().item;
                            //Debug.Log("�ε��� : " + index);
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
            // ����ǰ ������� ���������� �� ���¶��
            // ������ Ŭ���ϸ� �������� ������ ������ ������ �����츦 ȭ�鿡 ���
        }
    }
}