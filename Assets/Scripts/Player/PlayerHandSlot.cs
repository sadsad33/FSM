using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerHandSlot : MonoBehaviour {
        public Item? currentItemOnHand;
        GameObject? currentItemModelOnHand;

        public void EquipItemOnHand(Item item) {
            currentItemOnHand = item;
            if (item.itemModel != null)
                currentItemModelOnHand = Instantiate(item.itemModel, transform);
        }

        public void UnEquipItemOnHand(Item item) {
            currentItemOnHand = null;
            if (currentItemModelOnHand != null)
                Destroy(currentItemModelOnHand);
        }
    }
}