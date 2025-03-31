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
