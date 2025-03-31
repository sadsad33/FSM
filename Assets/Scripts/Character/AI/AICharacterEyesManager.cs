using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterEyesManager : MonoBehaviour {
        public Transform eyesTransform;
        public CharacterManager targetPossible;
        public CharacterManager targetAround;
        public CharacterManager currentTarget;
        AICharacterManager aiCharacterManager;

        private void Awake() {
            aiCharacterManager = GetComponent<AICharacterManager>();
        }

        private void Update() {
            LookingAhead();
            DetectingAround();
        }

        Collider[] colliders;
        private void DetectingAround() {
            colliders = Physics.OverlapSphere(transform.position, aiCharacterManager.aiStatsManager.detectionRadius, aiCharacterManager.aiStatsManager.detectionLayer);
            for (int i = 0; i < colliders.Length; i++) {
                CharacterManager character = colliders[i].transform.GetComponent<CharacterManager>();

                if (character != null && character.transform.gameObject != this.transform.gameObject) {
                    Vector3 targetDirection = character.transform.position - transform.position;
                    float angle = Vector3.Angle(targetDirection, transform.forward);
                    if (angle < 45 && angle > -45) {
                        //Debug.Log(character);
                        targetAround = character;
                    }
                }
            }
        }

        private void LookingAhead() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, aiCharacterManager.aiStatsManager.detectionRadius * 2)) {
                CharacterManager character = hit.transform.GetComponent<CharacterManager>();
                if (character != null) {
                    targetPossible = character;
                }
            }
        }
    }
}