using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class WorldEffectManager : MonoBehaviour {
        public static WorldEffectManager instance;

        // ������ ����Ʈ ������ ���
        // ������ ����Ʈ�� �߻��ϸ� �� ����� �̿��� �����͸� ����
        // ������� �����͸� �̿��� TakeDamageEffect�� ����
        public TakeDamageEffectData takeDamageEffectData;
        public TakeDamageEffect CreateTakeDamageEffect(CharacterManager from, TakeDamageEffectData data) {
            return new TakeDamageEffect(from, data);
        }

        void Awake() {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
    }
}
