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

    // ���ε� : ���۾Ƹ� ������ ���� �ʿ� ��ġ
    [Header("Poise")]
    public float totalPoiseDefense; // ������ ��꿡���� �� ���ε�
    public float offensivePoiseBonus; // ���ݸ���� ���ε� ���ʽ�
    public float armorPoiseBonus; // ������ �������ν� ��� ���ε�
    public float totalPoiseResetTime; // ���ε� �ʱ�ȭ �ð�
    public float poiseResetTimer = 0; // ���ε� �ʱ�ȭ Ÿ�̸�
    public bool isStuned; // �׷α� ����

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

    // ĳ���Ͱ� ���ϴ� �������� ����
    [Header("Damage Type Modifiers")]
    public float physicalDamagePercentageModifier = 100;
    public float fireDamagePercentageModifier = 100;

    // ĳ���Ͱ� �޴� �������� ����
    [Header("Damage Absorption Modifiers")]
    public float physicalAbsorptionPercentageModifier = 0;
    public float fireAbsorptionPercentageModifier = 0;

    [Header("Poison")]
    public bool isPoisoned;
    public float poisonBuildUp = 0; // �� ������ ���� ��ġ, �� ��ġ�� 100�� �Ǹ� �� �����̻� �ɸ���
    public float poisonAmount = 100; // �� ���¿��� �� �����̻��� �����Ǳ� ���� �ʿ��� ��ġ

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
