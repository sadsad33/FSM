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
                // ���߿� ������ ���ݴ��� ���
                player.psm.groundedHitState.SetDamageAnimation((currentEffect as TakeDamageEffect).GetDamageAnimation());
                player.psm.ChangeState(player.psm.groundedHitState);
            }
        }
    }
}
