using System;
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
        InventoryWindow invWin;
        public GameObject equipmentUI;


        [Header("상호작용 팝업창")]
        public GameObject interactionPopUp;
        public Text interactionPopUpMessage;

        [Header("아이템 습득 팝업창")]
        public GameObject itemPopUp;
        public Text itemPopUpMessage;
        public Image itemPopUpImage;

        //public Func<Item, Item> swapItem;
        private void Awake() {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            uiStack = new Stack<GameObject>();
            hudUI = transform.GetChild(0).GetChild(0).gameObject;
            healthBar = hudUI.transform.GetChild(0).GetComponent<HealthBar>();
            staminaBar = hudUI.transform.GetChild(1).GetComponent<StaminaBar>();

            menuSelectionUI = transform.GetChild(0).GetChild(1).gameObject;
            inventoryUI = transform.GetChild(0).GetChild(2).gameObject;
            invWin = inventoryUI.GetComponent<InventoryWindow>();
            equipmentUI = transform.GetChild(0).GetChild(3).gameObject;

            interactionPopUp = transform.GetChild(0).GetChild(4).gameObject;
            interactionPopUpMessage = interactionPopUp.transform.GetChild(0).GetComponent<Text>();

            itemPopUp = transform.GetChild(0).GetChild(5).gameObject;
            itemPopUpMessage = itemPopUp.transform.GetChild(0).GetComponent<Text>();
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
            if (player.isClimbing) interactionPopUp.SetActive(false);
        }

        public int GetUIStackSize() {
            return uiStack.Count;
        }

        public void HandleESCInput() {
            if (uiStack.Peek() == hudUI) {
                player.cursorSet = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                OpenWindow(menuSelectionUI);
            } else {
                CloseWindow();
            }
        }

        // 버튼 클릭 이벤트
        public void SelectWindow(int uiID) {
            uiStack.Peek().SetActive(false);
            switch (uiID) {
                case 0:
                    inventoryUI.GetComponent<InventoryWindow>().InventoryPageIndex = 0;
                    OpenWindow(inventoryUI);
                    break;
                case 1:
                    OpenWindow(equipmentUI);
                    break;
            }
        }

        public void OpenWindow(GameObject ui) {
            if (uiStack.Peek() == ui) return;
            if (uiStack.Peek() != hudUI) uiStack.Peek().SetActive(false);
            uiStack.Push(ui);
            ui.SetActive(true);
            //Debug.Log("Current Stack Top : " + uiStack.Peek());
        }

        public void CloseWindow() {
            uiStack.Peek().SetActive(false);
            uiStack.Pop();
            if (uiStack.Peek() != hudUI) {
                uiStack.Peek().SetActive(true);
            }
            //Debug.Log("Current Stack Top : " + uiStack.Peek());
        }

        // 장비 윈도우에 있는 아이템 슬롯을 클릭했을 경우
        public void SetHandlerToInventorySlot() {
            // 해당하는 장비의 소지품 페이지가 열림
            OpenWindow(inventoryUI);
            invWin.isChangingEquipment = true;
            // 인벤토리 창의 아이템 슬롯에 이벤트 핸들러 연결
            invWin.SetItemSlotClickEvents();
        }
    }
}