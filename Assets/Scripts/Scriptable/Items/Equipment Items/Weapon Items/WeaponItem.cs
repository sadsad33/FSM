using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class WeaponItem : EquipmentItem {
        // Stats
        public float physicalDamage;

        // 무기 관련 애니메이션(공격, 방어 등)
        // 약공격 배수, 강공격 배수, 치명적 일격 배수 등
        // 강인도 데미지
        public float poiseDamage;
        public float offensivePoiseBonus;
        // 방어 했을시 물리 경감률, 속성 경감률
        // 전투 기술 정보
        // 무기 사용시 소모되는 자원에 대한 정보(스태미너 등)
        // SFX 등
    }
}
