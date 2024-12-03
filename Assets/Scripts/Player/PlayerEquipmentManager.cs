using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour {
    public PlayerManager player;
    public List<Item> playerRightHandEquipmentSlots;
    public PlayerHandSlot playerRightHandSlot;
    public int playerCurrentRightHandSlotIndex;
    //public PlayerHandSlot playerLeftHandSlot;
    //public int playerLeftHandSlotIndex;

    private void Awake() {
        player = GetComponent<PlayerManager>();
        playerRightHandSlot = GetComponentInChildren<PlayerHandSlot>();
    }

    private void Start() {
        playerRightHandSlot.EquipItemOnHand(playerRightHandEquipmentSlots[playerCurrentRightHandSlotIndex]);
    }

    public void ChangeRightHandWeapon() {
        if (playerRightHandSlot.currentItemOnHand != null)
            playerRightHandSlot.UnEquipItemOnHand(playerRightHandSlot.currentItemOnHand);
        playerCurrentRightHandSlotIndex++;
        if (playerCurrentRightHandSlotIndex >= 3)
            playerCurrentRightHandSlotIndex -= 3;
        playerRightHandSlot.EquipItemOnHand(playerRightHandEquipmentSlots[playerCurrentRightHandSlotIndex]);
    }
}
