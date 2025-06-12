using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public static class Enums {
        public enum ItemCategoryCode {
            Consumable_Countless, Consumable_Countable,
            Equipment_Weapon_Melee, Equipment_Weapon_Range, Equipment_Weapon_Catalyst,
            Equipment_Armor_Helmet, Equipment_Armor_ChestArmor, Equipment_Armor_Gauntlets, Equipment_Armor_Greaves,
            Equipment_Accessory_Ring, Equipment_Accessory_Cape
        };

        public enum CharacterBehaviourCode { Strafe, LightAttack, HeavyAttack, ComboAttack, RunningAttack, Defend, Dodge, Parry, Interaction };

        public enum CharacterEffectCode { TakeDamage, Poison };
    }
}
  