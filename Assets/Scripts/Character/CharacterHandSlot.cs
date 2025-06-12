using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterHandSlot : MonoBehaviour {
        [SerializeField] Item? currentItemOnSlot;
        GameObject? currentItemModelOnSlot;
        [SerializeField] bool isLeftHandSlot;
        public virtual void EquipItemOnSlot(Item item) {
            if (item == null) return;
            currentItemOnSlot = item;
            if (item.itemModel != null)
                currentItemModelOnSlot = Instantiate(item.itemModel, transform);
        }

        public virtual void UnEquipItemOnSlot() {
            //Debug.Log("아이템 언로드");
            currentItemOnSlot = null;
            if (currentItemModelOnSlot != null)
                Destroy(currentItemModelOnSlot);
        }

        public bool IsLeftHandSlot() => isLeftHandSlot;

        public Item GetItemOnSlot() => currentItemOnSlot;

        public GameObject GetItemModelOnSlot() => currentItemModelOnSlot;
    }
}
