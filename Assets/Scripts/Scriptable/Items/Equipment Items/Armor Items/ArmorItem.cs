using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    [CreateAssetMenu(menuName = "Items/EquipmentItems/Armors")]
    public class ArmorItem : EquipmentItem {
        public int armorCode; // �θ� ������Ʈ�κ��� ���° �ڽĿ� �ִ���
        public float physicalAbsorption;
    }
}