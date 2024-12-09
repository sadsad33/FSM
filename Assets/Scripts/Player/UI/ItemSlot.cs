using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KBH {
    public class ItemSlot : MonoBehaviour {
        public Item item;
        public Image itemIcon;

        public void AddItem(Item item) {
            this.item = item;
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}