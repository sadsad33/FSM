using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class TakeDamageEffect : CharacterEffect {

        // TODO
        // SFX
        // 속성뎀
        readonly CharacterManager characterDamageFrom;
        readonly TakeDamageEffectData takeDamageEffectData;

        bool poiseIsBroken;
        string damageAnimation;

        public TakeDamageEffect(CharacterManager characterDamageFrom, TakeDamageEffectData takeDamageEffectData) : base(takeDamageEffectData) {
            this.characterDamageFrom = characterDamageFrom;
            this.takeDamageEffectData = takeDamageEffectData;
            poiseIsBroken = false;
        }

        public override void ProcessEffect(CharacterManager character) {
            if (character.characterStatsManager.isDead) return;
            if (character.isInvulnerable) return;
            CalculateDamage(character);
            // 강인도에 의해 자세가 무너졌는지에 따른 애니메이션 재생

            //if (poiseIsBroken)
            CheckWhichDirectionDamageCameFrom();
            //PlayDamageAnimation(character);
        }

        void CalculateDamage(CharacterManager character) {
            if (characterDamageFrom != null) {
                takeDamageEffectData.physicalDamage *= characterDamageFrom.characterStatsManager.physicalDamagePercentageModifier;
                // 속성 데미지?
            }

            float totalPhysicalDamageAbsorption = 1 -
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionBody) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands);
            takeDamageEffectData.physicalDamage -= (takeDamageEffectData.physicalDamage * totalPhysicalDamageAbsorption);

            takeDamageEffectData.physicalDamage -= takeDamageEffectData.physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100);

            // 속성 데미지?

            float finalDamage = takeDamageEffectData.physicalDamage; // + 속성 데미지
            character.characterStatsManager.currentHealth -= finalDamage;

            //Debug.Log("캐릭터의 강인도 : " + character.characterStatsManager.totalPoiseDefense);
            //Debug.Log("강인도 데미지 : " + takeDamageEffectData.poiseDamage);
            character.characterStatsManager.totalPoiseDefense -= takeDamageEffectData.poiseDamage;
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.totalPoiseResetTime;

            if (character.characterStatsManager.totalPoiseDefense <= 0) poiseIsBroken = true;

            if (character.characterStatsManager.currentHealth <= 0) {
                character.characterStatsManager.currentHealth = 0;
                character.characterStatsManager.isDead = true;
            }
        }

        void CheckWhichDirectionDamageCameFrom() {
            //if(character.isGrounded){
            if (takeDamageEffectData.angleHitFrom >= -30 && takeDamageEffectData.angleHitFrom <= 30) {
                damageAnimation = "Hit_From_Backward";
            } else if (takeDamageEffectData.angleHitFrom < -30 && takeDamageEffectData.angleHitFrom > -150) {
                damageAnimation = "Hit_From_Left";
            } else if (takeDamageEffectData.angleHitFrom < 30 && takeDamageEffectData.angleHitFrom < 150) {
                damageAnimation = "Hit_From_Right";
            } else {
                damageAnimation = "Hit_From_Forward";
            }
            //} else {
            //    damageAnimation = "Hit_Airborne";
            //}
        }

        public string GetDamageAnimation() => damageAnimation;
        
        // 피격 애니메이션 재생
        // 상태를 만들면 거기서 재생하는게 나을듯?
        //void PlayDamageAnimation(CharacterManager character) {
        //    //if (character.characterStatsManager.isDead) {
        //    //    // 캐릭터가 죽었을 경우
        //    //}
        //    //if (!poiseIsBroken) return;
        //    //else {
        //    //character.isPerformingAction = true;

        //    if (character.characterAnimatorManager.animator.GetCurrentAnimatorStateInfo(3).IsName(damageAnimation)) {
        //        //Debug.Log("애니메이션 강제 재생");
        //        character.characterAnimatorManager.animator.applyRootMotion = character.isPerformingAction;
        //        character.characterAnimatorManager.animator.Play(damageAnimation, 3, 0f);
        //        character.characterAnimatorManager.animator.Update(0);
        //    } else {
        //        //Debug.Log("애니메이션 재생");
        //        character.characterAnimatorManager.PlayAnimation(damageAnimation, character.isPerformingAction);
        //    }
        //}
    }
}