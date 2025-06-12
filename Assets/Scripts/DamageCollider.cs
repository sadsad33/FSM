using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class DamageCollider : MonoBehaviour {
        public CharacterManager characterCausingDamage;
        [SerializeField] protected Collider damageCollider;

        [Header("�� �ĺ���ȣ")]
        public int teamID;

        [Header("���ε�")]
        public float poiseDamage;
        public float offensivePoiseBonus;

        [Header("���ط�")]
        public float physicalDamage;
        //public float fireDamage;

        protected bool shieldHasBeenHit;
        bool hasBeenParried;
        protected string currentDamageAnimation;

        protected Vector3 contactPoint;
        protected float angleHitFrom;

        // ���� �ݶ��̴� ���� �ݱ�

        protected virtual void Awake() {
            damageCollider = GetComponentInChildren<Collider>();
        }
        
        public void EnableDamageCollider() {
            //Debug.Log("���� �ݶ��̴� ����");
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider() {
            damageCollider.enabled = false;
        }

        protected void OnTriggerEnter(Collider other) {
            //if (!characterCausingDamage.IsOwner) return;
            if (other.CompareTag("Character")) {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterManager damageTarget = other.GetComponent<CharacterManager>();
                //BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();
                //Debug.Log(damageTarget.characterStatsManager.isDead);

                if (damageTarget.characterStatsManager.isDead) return;
                //Debug.Log("passed DeathCheck");
                if (damageTarget != null) {
                    if (damageTarget.characterStatsManager != null) {
                        if (damageTarget.characterStatsManager.teamID == teamID) return;
                        CheckForParry(damageTarget);
                        //CheckForBlock(damageTarget, shield, damageTarget.characterStatsManager);
                        if (hasBeenParried) return;
                        if (shieldHasBeenHit) return;
                        damageTarget.characterStatsManager.poiseResetTimer = damageTarget.characterStatsManager.totalPoiseResetTime; // ���ε� ���� �ð� ����
                        damageTarget.characterStatsManager.totalPoiseDefense -= poiseDamage;

                        //if (damageTarget.characterStatsManager.teamID == 0) {
                        //    NPCManager npcManager = damageTarget.transform.GetComponent<NPCManager>();
                        //    if (teamIDNumber == 1) {
                        //        npcManager.aggravationToEnemy += 30;
                        //    } else if (teamIDNumber == 2) {
                        //        npcManager.aggravationToPlayer += 10;
                        //    }
                        //    if (npcManager.currentTarget != characterCausingDamage.transform.GetComponent<CharacterStatsManager>()) {
                        //        npcManager.changeTargetTimer -= (npcManager.changeTargetTime / 2f);
                        //    }
                        //}

                        contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                        angleHitFrom = (Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up));
                    }
                }
            }

            //if (other.CompareTag("IllusionaryWall")) {
            //    IllusionaryWall illusionaryWall = other.GetComponent<IllusionaryWall>();
            //    illusionaryWall.illusionaryWallHealthPoint -= 1;
            //}
        }

        protected virtual void DealDamage(CharacterManager target) {
            TakeDamageEffectData takeDamageEffectData = Instantiate(WorldEffectManager.instance.takeDamageEffectData);
            takeDamageEffectData.physicalDamage = physicalDamage;
            takeDamageEffectData.poiseDamage = poiseDamage;
            TakeDamageEffect takeDamageEffect = WorldEffectManager.instance.CreateTakeDamageEffect(characterCausingDamage, takeDamageEffectData, angleHitFrom, contactPoint);
            target.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
        }

        protected virtual void CheckForParry(CharacterManager enemyManager) {
            if (enemyManager.isParrying) {
                //Debug.Log("�и� ����");
                characterCausingDamage.transform.GetComponent<CharacterAnimatorManager>().PlayAnimation("Parried", true);
                hasBeenParried = true;
            }
        }
    }
}
