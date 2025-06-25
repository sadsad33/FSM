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
            if (currentEffect.GetEffectData().GetEffectID() == Enums.CharacterEffectCode.TakeDamage) {
                // TODO
                // ���߿� ������ ���ݴ��� ���
                player.pmsm.ChangeState(player.pmsm.groundedHitState);
            }
            effect.ProcessEffect(player);
        }
    }
}
