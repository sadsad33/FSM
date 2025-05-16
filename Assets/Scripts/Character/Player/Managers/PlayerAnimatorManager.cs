using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerAnimatorManager : CharacterAnimatorManager {
        public PlayerManager player;
        
        protected override void Awake() {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void PlayAnimation(string animation, bool isInteracting) {
            base.PlayAnimation(animation, isInteracting);
            Debug.Log("플레이어의 현재 애니메이션 : " + animation);
        }
    }
}
