using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class TakeDamageEffect : CharacterEffect {

        // TODO
        // SFX
        // �Ӽ���
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
            // ���ε��� ���� �ڼ��� ������������ ���� �ִϸ��̼� ���

            //if (poiseIsBroken)
            CheckWhichDirectionDamageCameFrom();
            //PlayDamageAnimation(character);
        }

        void CalculateDamage(CharacterManager character) {
            if (characterDamageFrom != null) {
                takeDamageEffectData.physicalDamage *= characterDamageFrom.characterStatsManager.physicalDamagePercentageModifier;
                // �Ӽ� ������?
            }

            float totalPhysicalDamageAbsorption = 1 -
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionBody) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands);
            takeDamageEffectData.physicalDamage -= (takeDamageEffectData.physicalDamage * totalPhysicalDamageAbsorption);

            takeDamageEffectData.physicalDamage -= takeDamageEffectData.physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100);

            // �Ӽ� ������?

            float finalDamage = takeDamageEffectData.physicalDamage; // + �Ӽ� ������
            character.characterStatsManager.currentHealth -= finalDamage;

            //Debug.Log("ĳ������ ���ε� : " + character.characterStatsManager.totalPoiseDefense);
            //Debug.Log("���ε� ������ : " + takeDamageEffectData.poiseDamage);
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
        
        // �ǰ� �ִϸ��̼� ���
        // ���¸� ����� �ű⼭ ����ϴ°� ������?
        //void PlayDamageAnimation(CharacterManager character) {
        //    //if (character.characterStatsManager.isDead) {
        //    //    // ĳ���Ͱ� �׾��� ���
        //    //}
        //    //if (!poiseIsBroken) return;
        //    //else {
        //    //character.isPerformingAction = true;

        //    if (character.characterAnimatorManager.animator.GetCurrentAnimatorStateInfo(3).IsName(damageAnimation)) {
        //        //Debug.Log("�ִϸ��̼� ���� ���");
        //        character.characterAnimatorManager.animator.applyRootMotion = character.isPerformingAction;
        //        character.characterAnimatorManager.animator.Play(damageAnimation, 3, 0f);
        //        character.characterAnimatorManager.animator.Update(0);
        //    } else {
        //        //Debug.Log("�ִϸ��̼� ���");
        //        character.characterAnimatorManager.PlayAnimation(damageAnimation, character.isPerformingAction);
        //    }
        //}
    }
}