using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterEffectsManager : CharacterEffectsManager {

        AICharacterManager aiCharacter;
        protected override void Awake() {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void ProcessEffectInstantly(CharacterEffect effect) {
            base.ProcessEffectInstantly(effect);
            effect.ProcessEffect(character);
            if (currentEffect.GetEffectData().GetEffectID() == Enums.CharacterEffectCode.TakeDamage) {
                // TODO
                // 공중에 있을때 공격당할 경우
                aiCharacter.acsm.aiGroundedHitState.SetDamageAnimation((currentEffect as TakeDamageEffect).GetDamageAnimation());
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiGroundedHitState);
            }
        }
    }
}
