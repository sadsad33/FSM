using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    [CreateAssetMenu(menuName = "Items/EquipmentItems/Armors")]
    public class ArmorItem : EquipmentItem {
        public float physicalAbsorption;
        public int armorCode;
    }
}