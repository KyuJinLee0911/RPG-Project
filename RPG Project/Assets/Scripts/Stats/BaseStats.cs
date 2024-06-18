using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Saving;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1, 100)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] int currentLevel = 0;

        [SerializeField] float maxExpBefore = 0;
        [SerializeField] ParticleSystem levelUpParticle;
        public float maxExp { get => maxExpBefore; }

        public event Action OnLevelUp;

        private void Awake()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();

            Debug.Log(currentLevel);
        }

        private void Start()
        {

            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExperienceGained += HasLevelUp;
            }
        }

        private void HasLevelUp()
        {
            int newLevel = CalculateLevel();
            if (currentLevel < newLevel)
            {
                currentLevel = newLevel;
                levelUpParticle.Clear();
                levelUpParticle.Play();
                OnLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, CalculateLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();
            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = GetComponent<Experience>().exp;
            int maxLevel = progression.GetValues(Stat.ExpToNextLevel, characterClass);
            for (int i = currentLevel; i <= maxLevel; i++)
            {
                if (currentXP >= progression.GetStat(Stat.ExpToNextLevel, characterClass, i))
                {
                    maxExpBefore = progression.GetStat(Stat.ExpToNextLevel, characterClass, i);
                    continue;
                }
                else
                {
                    return i;
                }
            }
            return maxLevel + 1;
        }

        public object CaptureState()
        {
            return currentLevel;
        }

        public void RestoreState(object state)
        {
            currentLevel = (int)state;
        }
    }
}
