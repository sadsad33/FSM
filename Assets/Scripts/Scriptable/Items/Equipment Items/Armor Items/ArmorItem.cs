using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    [CreateAssetMenu(menuName = "Items/EquipmentItems/Armors")]
    public class ArmorItem : EquipmentItem {
        public int armorCode; // 부모 오브젝트로부터 몇번째 자식에 있는지
        public float physicalAbsorption;
    }
}