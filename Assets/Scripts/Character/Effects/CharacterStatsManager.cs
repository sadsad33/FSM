using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public int healthLevel;
    public int staminaLevel;

    public float maxHealth;
    public float maxStamina;

    public float currentHealth;
    public float currentStamina;

    public bool isDead = false;

    // 강인도 : 슈퍼아머 유지를 위한 필요 수치
    [Header("Poise")]
    public float totalPoiseDefense; // 데미지 계산에서의 총 강인도
    public float offensivePoiseBonus; // 공격모션중 강인도 보너스
    public float armorPoiseBonus; // 갑옷을 입음으로써 얻는 강인도
    public float totalPoiseResetTime; // 강인도 초기화 시간
    public float poiseResetTimer = 0; // 강인도 초기화 타이머
    public bool isStuned; // 그로기 상태

    [Header("Armor Absorptions")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionHands;
    public float totalPhysicalDamageDefenseRate;

    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionLegs;
    public float fireDamageAbsorptionHands;
    public float totalFireDamageDefenseRate;

    [Header("Resistances")]
    public float poisonResistance;

    // 캐릭터가 가하는 데미지의 배율
    [Header("Damage Type Modifiers")]
    public float physicalDamagePercentageModifier = 100;
    public float fireDamagePercentageModifier = 100;

    // 캐릭터가 받는 데미지의 배율
    [Header("Damage Absorption Modifiers")]
    public float physicalAbsorptionPercentageModifier = 0;
    public float fireAbsorptionPercentageModifier = 0;

    [Header("Poison")]
    public bool isPoisoned;
    public float poisonBuildUp = 0; // 독 축적을 위한 수치, 이 수치가 100이 되면 독 상태이상에 걸린다
    public float poisonAmount = 100; // 독 상태에서 독 상태이상이 해제되기 위해 필요한 수치

    protected virtual void Awake() {
        SetMaxHealth();
        SetMaxStamina();
    }

    private void SetMaxHealth() {
        currentHealth = healthLevel * 10;
        maxHealth = currentHealth;
    }

    private void SetMaxStamina() {
        currentStamina = staminaLevel * 10;
        maxStamina = currentStamina;
    }

    public void DeductHealth(float deduct) {
        currentHealth -= deduct;
    }
}
