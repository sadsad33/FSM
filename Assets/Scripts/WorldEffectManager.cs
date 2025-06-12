using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class WorldEffectManager : MonoBehaviour {
        public static WorldEffectManager instance;

        // 데미지 이펙트 데이터 양식
        // 데미지 이펙트가 발생하면 이 양식을 이용해 데이터를 생성
        // 만들어진 데이터를 이용해 TakeDamageEffect를 생성
        public TakeDamageEffectData takeDamageEffectData;
        public TakeDamageEffect CreateTakeDamageEffect(CharacterManager from, TakeDamageEffectData data, float angle, Vector3 point) {
            return new TakeDamageEffect(from, data, angle, point);
        }

        void Awake() {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
    }
}
