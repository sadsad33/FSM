using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public void UpdateStaminaBar(float stamina) {
        slider.value = stamina;
    }
}
