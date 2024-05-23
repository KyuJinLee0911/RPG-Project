using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] int currentLevel = 1;

        [SerializeField] float maxExpBefore = 0;
        public float maxExp { get => maxExpBefore; }

        private void Update()
        {
            if (gameObject.CompareTag("Player"))
                currentLevel = GetLevel();
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, currentLevel);
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if(experience == null) return startingLevel;

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

    }
}
