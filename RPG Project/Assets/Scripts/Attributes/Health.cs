using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        bool isDead = false;

        public bool IsDead { get => isDead; }
        [SerializeField] float health = 100;
        public float hp { get => health; }
        [SerializeField] float maxHealth = 100f;
        public float maxHp { get => maxHealth; }

        private void Start()
        {
            if(!isDead) health = GetComponent<BaseStats>().GetStat(Stat.Health);
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health); 

            BaseStats baseStats = GetComponent<BaseStats>();
            if (baseStats != null)
            {
                baseStats.OnLevelUp += LevelUpHpUpdate;
            }
        }

        private void LevelUpHpUpdate()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (isDead) return;

            health = Mathf.Max(health - damage, 0);
            Debug.Log(health);
            if (health <= 0)
            {
                Die();
                AwardExperience(instigator);
                return;
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            float _exp = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            experience.GainExperience(_exp);
        }

        public void Die()
        {
            if (health > 0) return;
            if (isDead) return;

            health = 0;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0) Die();
        }
    }
}