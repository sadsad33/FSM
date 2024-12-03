using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    public PlayerManager player;

    private Stack uiStack;
    public GameObject hudUI;
    public HealthBar healthBar;
    public StaminaBar staminaBar;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        hudUI = transform.GetChild(0).GetChild(0).gameObject;
        healthBar = hudUI.transform.GetChild(0).GetComponent<HealthBar>();
        staminaBar = hudUI.transform.GetChild(1).GetComponent<StaminaBar>();

        healthBar.slider.maxValue = player.playerStatsManager.maxHealth;
        healthBar.slider.value = healthBar.slider.maxValue;
        staminaBar.slider.maxValue = player.playerStatsManager.maxStamina;
        staminaBar.slider.value = staminaBar.slider.maxValue;
    }

    private void Update() {
        healthBar.UpdateHealthBar(player.playerStatsManager.currentHealth);
        staminaBar.UpdateStaminaBar(player.playerStatsManager.currentStamina);
    }
}
