using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class Item : ScriptableObject {
        public ItemCategory itemCategory;
        public Sprite itemIcon;
        public GameObject? itemModel;
        public string itemName;
        public string flavorText;
    }
}