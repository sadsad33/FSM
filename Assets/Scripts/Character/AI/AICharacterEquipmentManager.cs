using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterEquipmentManager : CharacterEquipmentManager {

        AICharacterManager aiCharacterManager;
        // AI 캐릭터가 기본적으로 들고있을 무기
        [SerializeField] WeaponItem aiWeapon;
        protected override void Awake() {
            base.Awake();
            aiCharacterManager = GetComponent<AICharacterManager>();
            rightHandEquipments = new(3) { aiWeapon, null, null };
            leftHandEquipments = new(3) { null, null, null };
            characterArmorEquipments = new(4) { null, null, null, null };
        }

        protected override void Start() {
            base.Start();
            if (!aiCharacterManager.aiStatsManager.hasDrawnWeapon) {
                rightHandSlot.UnEquipItemOnSlot();
                LoadRightWeaponDamageCollider();
            }
        }

        public WeaponItem GetAICurrentRightWeapon() => aiWeapon;
    }
}
