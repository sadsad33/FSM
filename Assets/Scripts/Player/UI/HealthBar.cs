using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public void UpdateHealthBar(float health) {
        slider.value = health;
    }
}   