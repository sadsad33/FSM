using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterHandSlot : MonoBehaviour {
        [SerializeField] Item? currentItemOnSlot;
        GameObject? currentItemModelOnSlot;
        [SerializeField] bool isLeftHandSlot;
        [SerializeField] Item unarmed;

        public virtual void EquipItemOnSlot(Item item) {
            if (item == null) currentItemOnSlot = unarmed;
            else currentItemOnSlot = item;

            if (currentItemModelOnSlot != null) Destroy(currentItemModelOnSlot);

            if (currentItemOnSlot.itemModel != null)
                currentItemModelOnSlot = Instantiate(currentItemOnSlot.itemModel, transform);
        }

        public virtual void UnEquipItemOnSlot() {
            //Debug.Log("아이템 언로드");
            if (currentItemModelOnSlot != null)
                Destroy(currentItemModelOnSlot);
            EquipItemOnSlot(unarmed);
        }

        public bool IsLeftHandSlot() => isLeftHandSlot;

        public Item GetItemOnSlot() {
            //if (currentItemOnSlot == unarmed) return null;
            return currentItemOnSlot;
        }

        public GameObject GetItemModelOnSlot() => currentItemModelOnSlot;
    }
}
