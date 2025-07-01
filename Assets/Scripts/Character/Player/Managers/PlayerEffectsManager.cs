using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerEffectsManager : CharacterEffectsManager {

        PlayerManager player;
        
        protected override void Awake() {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void ProcessEffectInstantly(CharacterEffect effect) {
            base.ProcessEffectInstantly(effect);
            effect.ProcessEffect(player);
            if (currentEffect.GetEffectData().GetEffectID() == Enums.CharacterEffectCode.TakeDamage) {
                // TODO
                // 공중에 있을때 공격당할 경우
                player.psm.groundedHitState.SetDamageAnimation((currentEffect as TakeDamageEffect).GetDamageAnimation());
                player.psm.ChangeState(player.psm.groundedHitState);
            }
        }
    }
}
