using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
[CreateAssetMenu(menuName = "Items/EquipmentItems/MeleeWeaponItems")]
    public class MeleeWeaponItem : WeaponItem {
        //아이템의 스탯

        public string[] lightAttackAnimations;
        public string[] heavyAttackAnimations;

        public string jumpAttackAnimation;
        public string slidAttackAnimation;
        public string crouchAttackAnimation;
        public string runningAttackAnimation;
    }
}
