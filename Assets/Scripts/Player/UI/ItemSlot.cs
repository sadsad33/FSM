using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KBH {
    public delegate void ItemSlotButtonClickEvent();
    public class ItemSlot : MonoBehaviour, IPointerClickHandler {
        public Item item;
        public Image itemIcon;
        public Button button;
        public event ItemSlotButtonClickEvent itemSlotButtonLeftClickListener;
        public event ItemSlotButtonClickEvent itemSlotButtonRightClickListener;

        public void AddItem(Item item) {
            if (item == null) return;
            this.item = item;
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = new Color(1f, 1f, 1f, 1f);
        }

        public void ClearItem() {
            //Debug.Log("½½·Ô ºñ¿ì±â");
            item = null;
            itemIcon.sprite = null;
            itemIcon.color = new Color(0f, 0f, 0f, 0f);
        }

        public bool LeftClickEventIsEmpty() {
            return itemSlotButtonLeftClickListener == null;
        }

        public bool RightClickEventIsEmpty() {
            return itemSlotButtonRightClickListener == null;
        }

        public void PrintRightClickEventList() {
            if (itemSlotButtonRightClickListener == null) {
                return;
            }
            Delegate[] del = itemSlotButtonRightClickListener.GetInvocationList();
            for (int i = 0; i < del.Length; i++) {
                Debug.Log(del[i]);
            }
        }

        public void PrintleftClickEventList() {
            if (itemSlotButtonLeftClickListener == null) {
                return; 
            }
            Delegate[] del = itemSlotButtonLeftClickListener.GetInvocationList();
            for (int i = 0; i < del.Length; i++) {
                Debug.Log(del[i]);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                itemSlotButtonLeftClickListener();
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                if (itemSlotButtonRightClickListener.GetInvocationList() != null)
                    itemSlotButtonRightClickListener();
            }
        }

        public void ResetItemSlotButtonRightClickEvent() {
            itemSlotButtonRightClickListener = null;
        }

        public void ResetItemSlotButtonLeftClickEvent() {
            itemSlotButtonLeftClickListener = null;
        }
    }
}