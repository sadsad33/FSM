using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEquipmentManager : MonoBehaviour {
        public CharacterManager character;
        [Header("RightHand")]
        public List<Item> rightHandEquipments;
        public CharacterHandSlot rightHandSlot;
        public int currentRightHandSlotIndex;
        public DamageCollider rightHandDamageCollider;

        [Header("LeftHand")]
        public List<Item> leftHandEquipments;
        public CharacterHandSlot leftHandSlot;
        public int currentLeftHandSlotIndex;
        public DamageCollider leftHandDamageCollider;

        [Header("Armor")]
        public List<ArmorItem> characterArmorEquipments;
        public Transform characterFaceModel;
        public Transform characterHelmetModel;
        public Transform characterChestArmorModel;
        //public Transform characterCapeModel;
        public Transform pcharacterGauntletsModel;
        public Transform characterGreavesModel;
        public Transform characterBootsModel;

        [Header("Consumable")]
        public List<Item> consumableEquipments;

        [Header("Accessory")]
        public List<Item> accessoryEquipments;
        protected virtual void Awake() {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start() {
            LoadHandSlots();
            //Debug.Log("¿À¸¥¼Õ¿¡ ¹«±â ÀåÂø");
            rightHandSlot.EquipItemOnSlot(rightHandEquipments[currentRightHandSlotIndex]);
            LoadRightWeaponDamageCollider();
            leftHandSlot.EquipItemOnSlot(leftHandEquipments[currentLeftHandSlotIndex]);
            LoadLeftWeaponDamageCollider();
        }

        public void SetHelmet() {
            for (int i = 0; i < characterHelmetModel.childCount; i++) {
                characterHelmetModel.GetChild(i).gameObject.SetActive(false);
            }
            if (characterArmorEquipments[0] == null) characterHelmetModel.GetChild(0).gameObject.SetActive(true);
            else characterHelmetModel.GetChild(characterArmorEquipments[0].armorCode).gameObject.SetActive(true);
        }

        public void SetChestArmor() {
            for (int i = 0; i < characterChestArmorModel.childCount; i++) {
                characterChestArmorModel.GetChild(i).gameObject.SetActive(false);
            }
            if (characterArmorEquipments[1] == null) characterChestArmorModel.GetChild(0).gameObject.SetActive(true);
            else characterChestArmorModel.GetChild(characterArmorEquipments[1].armorCode).gameObject.SetActive(true);
        }

        public void SetGauntlets() {
            for (int i = 0; i < pcharacterGauntletsModel.childCount; i++) {
                pcharacterGauntletsModel.GetChild(i).gameObject.SetActive(false);
            }
            if (characterArmorEquipments[2] == null) pcharacterGauntletsModel.GetChild(0).gameObject.SetActive(true);
            else pcharacterGauntletsModel.GetChild(characterArmorEquipments[2].armorCode).gameObject.SetActive(true);
        }

        public void SetGreaves() {
            for (int i = 0; i < characterGreavesModel.childCount; i++) {
                characterGreavesModel.GetChild(i).gameObject.SetActive(false);
                characterBootsModel.GetChild(i).gameObject.SetActive(false);
            }
            if (characterArmorEquipments[3] == null) {
                characterGreavesModel.GetChild(0).gameObject.SetActive(true);
                characterBootsModel.GetChild(0).gameObject.SetActive(true);
            } else {
                characterGreavesModel.GetChild(characterArmorEquipments[3].armorCode).gameObject.SetActive(true);
                characterBootsModel.GetChild(characterArmorEquipments[3].armorCode).gameObject.SetActive(true);
            }
        }

        protected void LoadHandSlots() {
            CharacterHandSlot[] handSlots = GetComponentsInChildren<CharacterHandSlot>();
            foreach (CharacterHandSlot handSlot in handSlots) {
                if (handSlot.IsLeftHandSlot()) leftHandSlot = handSlot;
                else rightHandSlot = handSlot;
            }
        }

        public virtual void LoadRightWeaponDamageCollider() {
            rightHandDamageCollider = rightHandSlot.GetItemModelOnSlot().GetComponentInChildren<DamageCollider>();
            WeaponItem temp = rightHandSlot.GetItemOnSlot() as WeaponItem;
            rightHandDamageCollider.physicalDamage = temp.physicalDamage;
            rightHandDamageCollider.poiseDamage = temp.poiseDamage;
            rightHandDamageCollider.characterCausingDamage = character;
            rightHandDamageCollider.teamID = character.characterStatsManager.teamID;
            rightHandDamageCollider.DisableDamageCollider();
        }

        public virtual void LoadLeftWeaponDamageCollider() {
            leftHandDamageCollider = leftHandSlot.GetItemModelOnSlot().GetComponentInChildren<DamageCollider>();
            WeaponItem temp = leftHandSlot.GetItemOnSlot() as WeaponItem;
            leftHandDamageCollider.physicalDamage = temp.physicalDamage;
            leftHandDamageCollider.poiseDamage = temp.poiseDamage;
            leftHandDamageCollider.characterCausingDamage = character;
            leftHandDamageCollider.teamID = character.characterStatsManager.teamID;
            leftHandDamageCollider.DisableDamageCollider();
        }

        public void OpenDamageCollider() {
            //Debug.Log("¿­·È´Ù");
            if (rightHandDamageCollider != null) rightHandDamageCollider.EnableDamageCollider();
            if (leftHandDamageCollider != null) leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider() {
            //Debug.Log("´ÝÇû´Ù");
            if (rightHandDamageCollider != null) rightHandDamageCollider.DisableDamageCollider();
            if (leftHandDamageCollider != null) leftHandDamageCollider.DisableDamageCollider();
        }


    }
}
