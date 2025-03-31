using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerHandSlot : MonoBehaviour {
        public Item? currentItemOnSlot;
        GameObject? currentItemModelOnSlot;

        public void EquipItemOnSlot(Item item) {
            if (item == null) return;
            currentItemOnSlot = item;
            if (item.itemModel != null)
                currentItemModelOnSlot = Instantiate(item.itemModel, transform);
        }

        public void UnEquipItemOnSlot() {
            //Debug.Log("������ ��ε�");
            currentItemOnSlot = null;
            if (currentItemModelOnSlot != null)
                Destroy(currentItemModelOnSlot);
        }
    }
}