using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterEquipmentManager : CharacterEquipmentManager {

        [SerializeField] WeaponItem rightWeapon; 
        protected override void Awake() {
            base.Awake();
            rightHandEquipments = new(3) { rightWeapon, null, null };
            leftHandEquipments = new(3) { null, null, null };
            characterArmorEquipments = new(4) { null, null, null, null };
        }

        protected override void Start() {
            base.Start();
        }
    }
}
