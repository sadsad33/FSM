using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KBH {
    public class PlayerUIManager : MonoBehaviour {
        public static PlayerUIManager instance;
        public PlayerManager player;

        private Stack<GameObject> uiStack;
        public GameObject hudUI;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public GameObject menuSelectionUI;
        public GameObject inventoryUI;
        public GameObject equipmentUI;

        [Header("상호작용 팝업창")]
        public GameObject interactionPopUp;
        public Text interactionPopUpMessage;

        [Header("아이템 습득 팝업창")]
        public GameObject itemPopUp;
        public Text itemPopUpMessage;
        public Image itemPopUpImage;

        public InventoryWindow inventoryWindow;
        private void Awake() {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            uiStack = new Stack<GameObject>();
            hudUI = transform.GetChild(0).GetChild(0).gameObject;
            healthBar = hudUI.transform.GetChild(0).GetComponent<HealthBar>();
            staminaBar = hudUI.transform.GetChild(1).GetComponent<StaminaBar>();

            menuSelectionUI = transform.GetChild(0).GetChild(1).gameObject;
            inventoryUI = transform.GetChild(0).GetChild(2).gameObject;
            equipmentUI = transform.GetChild(0).GetChild(3).gameObject;

            interactionPopUp = transform.GetChild(0).GetChild(4).gameObject;
            interactionPopUpMessage = interactionPopUp.transform.GetChild(0).GetComponent<Text>();

            itemPopUp = transform.GetChild(0).GetChild(5).gameObject;
            itemPopUpMessage = transform.GetChild(0).GetComponent<Text>();
            itemPopUpImage = itemPopUp.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }

        private void Start() {
            healthBar.slider.maxValue = player.playerStatsManager.maxHealth;
            healthBar.slider.value = healthBar.slider.maxValue;
            staminaBar.slider.maxValue = player.playerStatsManager.maxStamina;
            staminaBar.slider.value = staminaBar.slider.maxValue;

            uiStack.Push(hudUI);
        }

        private void Update() {
            healthBar.UpdateHealthBar(player.playerStatsManager.currentHealth);
            instance.staminaBar.UpdateStaminaBar(player.playerStatsManager.currentStamina);
            
        }

        public void HandleESCInput() {
            if (uiStack.Peek() == hudUI) {
                OpenMenuSelectionWindow();
            }
            else {
                CloseWindow();
            }
        }

        public void OpenMenuSelectionWindow() {
            uiStack.Push(menuSelectionUI);
            menuSelectionUI.SetActive(true);
        }

        // 버튼 클릭 이벤트
        public void OpenSelectedWindow(float uiID) {
            uiStack.Peek().SetActive(false);
            switch (uiID) {
                case 0:
                    uiStack.Push(inventoryUI);
                    inventoryUI.SetActive(true);
                    break;
                case 1:
                    uiStack.Push(equipmentUI);
                    equipmentUI.SetActive(true);
                    break;
            }
        }

        public void CloseWindow() {
            GameObject currentWindow = uiStack.Peek();
            uiStack.Pop();
            currentWindow.SetActive(false);
            if (uiStack.Peek() != hudUI) uiStack.Peek().SetActive(true);
        }
    }
}