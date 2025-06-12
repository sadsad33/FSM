using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEffectData : ScriptableObject {
        [SerializeField] Enums.CharacterEffectCode effectID;

        public Enums.CharacterEffectCode GetEffectID() => effectID;
    }
}