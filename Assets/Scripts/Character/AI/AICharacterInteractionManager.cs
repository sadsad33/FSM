using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterInteractionManager : CharacterInteractionManager {

        public GameObject riposteCollider;
        public GameObject backStabCollider;
        protected override void Awake() {
            base.Awake();
            riposteCollider = transform.GetChild(1).gameObject;
            riposteCollider.SetActive(false);
            backStabCollider = transform.GetChild(2).gameObject;
        }

        protected override void OnTriggerStay(Collider other) {
            if (other.CompareTag("Interactable")) {
                //Debug.Log("��ȣ�ۿ� ���� ��ü ����");
                currentInteractable = other.GetComponent<Interactable>();
                //Debug.Log(currentInteractable);
            }
        }

        protected override void OnTriggerExit(Collider other) {
            //Debug.Log("��ȣ�ۿ� ���� ��ü���� �־���");
        }
    }
}
