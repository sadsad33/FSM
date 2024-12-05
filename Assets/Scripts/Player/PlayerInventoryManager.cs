using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {
    public List<Item> playerInventorySlots;

    private void Awake() {
        playerInventorySlots = new List<Item>();
    }

    public void AddItem(Item item) {
        playerInventorySlots.Add(item);
    }
}
