using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public GameObject itemSlotModel;

    public GameObject inventoryIndex;
    public GameObject inventoryPage;
    public List<GameObject> itemSlots;

    private void OnEnable() {
        itemSlots = new List<GameObject>();
        inventoryIndex = transform.GetChild(0).GetChild(0).gameObject;
        inventoryPage = transform.GetChild(0).GetChild(1).gameObject;

        PlayerUIManager.instance.inventoryWindow = this;
        if (PlayerUIManager.instance.player.playerInventoryManager.playerInventorySlots.Count >= PlayerUIManager.instance.inventoryWindow.itemSlots.Count) {
            PlayerUIManager.instance.inventoryWindow.AddItemSlot();
        }
    }

    public void AddItemSlot() {
        itemSlots.Add(Instantiate(itemSlotModel, inventoryPage.transform));
    }
}
